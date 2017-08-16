using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models;
using Microsoft.EntityFrameworkCore;

namespace hitfit.app.Managers
{
    public class UserProgramManager
    {
        private readonly HitFitDbContext _context;

        public UserProgramManager(HitFitDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddUserProgramAsync(UserProgram userProgram)
        {
            int userProgramId;

            if (await _context.FindAsync<User>(userProgram.UserId) == null)
            {
                throw new Exception("User not found");
            }

            if (await _context.Programs.FindAsync(userProgram.ProgramId) == null)
            {
                throw new Exception("Program not found");
            }

            var userProgramEntity =
                await _context.UsersPrograms.FirstOrDefaultAsync(
                    p => p.UserId == userProgram.UserId && p.ProgramId == userProgram.ProgramId && p.IsActive == true);

            if (userProgramEntity == null)
            {
                await _context.UsersPrograms.AddAsync(userProgram);

                userProgramId = userProgram.Id;
            }
            else
            {
                userProgramEntity.IsActive = userProgram.IsActive;
                userProgramEntity.StartedOn = userProgram.StartedOn;
                userProgramEntity.FinishedOn = userProgram.FinishedOn;
                userProgramEntity.Notes = userProgram.Notes;

                userProgramId = userProgramEntity.Id;
            }

            await _context.SaveChangesAsync();

            return userProgramId;
        }

        public async Task<UserProgram> GetClientProgramAsync(int userProgramId)
        {
            return await _context.UsersPrograms.FirstOrDefaultAsync(p => p.Id.Equals(userProgramId));
        }

        public async Task<List<UserProgram>> GetClientProgramsAsync(int? userId = null, int? programId = null)
        {
            IQueryable<UserProgram> query = _context.UsersPrograms;

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId.Equals(userId.Value));
            }

            if (programId.HasValue)
            {
                query = query.Where(p => p.ProgramId.Equals(programId.Value));
            }

            return await query.ToListAsync();
        }

        public async Task<List<UserProgram>> GetClientProgramsAsync(DateTime? startedOnFrom = null, DateTime? startedOnTo = null)
        {
            IQueryable<UserProgram> query = _context.UsersPrograms;

            if (startedOnFrom.HasValue)
            {
                query = query.Where(p => p.StartedOn >= startedOnFrom.Value);
            }

            if (startedOnTo.HasValue)
            {
                query = query.Where(p => p.StartedOn <= startedOnTo.Value);
            }

            return await query.ToListAsync();
        }

        //public async Task UpdateClientProgramAsync(UserProgram userProgram)
        //{
        //    var entity = await _context.UsersPrograms.FirstOrDefaultAsync(p => p.Id.Equals(userProgram.Id));

        //    if (entity == null)
        //    {
        //        return;
        //    }

        //    entity.UserId = userProgram.UserId;
        //    entity.ProgramId = userProgram.ProgramId;
        //    entity.StartedOn = userProgram.StartedOn;
        //    entity.FinishedOn = userProgram.FinishedOn;
        //    entity.Notes = userProgram.Notes;
        //    entity.ModifiedOn = DateTime.Now;

        //    await _context.SaveChangesAsync();
        //}
    }
}
