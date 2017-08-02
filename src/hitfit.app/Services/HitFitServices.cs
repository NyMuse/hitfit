using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models;
using Microsoft.EntityFrameworkCore;

namespace hitfit.app.Services
{
    public class HitFitServices : IDbImageService, IClientService
    {
        private readonly HitFitDbContext _context;

        public HitFitServices(HitFitDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveImageAsync(ImageObject image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();

            return image.Id;
        }

        public async Task<ImageObject> GetImageAsync(int id)
        {
            return await _context.Images.FirstOrDefaultAsync(i => i.Id.Equals(id));
        }

        public async Task<List<ImageObject>> GetImagesAsync(int ownerId, string relationType)
        {
            if (ownerId == 0 || string.IsNullOrEmpty(relationType))
            {
                return null;
            }

            return await _context.Images.Where(i => i.OwnerId.Equals(ownerId) && i.RelationType.Equals(relationType)).ToListAsync();
        }



        public async Task<int> AddClientProgramAsync(UserProgram userProgram)
        {
            await _context.UsersPrograms.AddAsync(userProgram);
            await _context.SaveChangesAsync();

            return userProgram.Id;
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

        public async Task UpdateClientProgramAsync(UserProgram userProgram)
        {
            var entity = await _context.UsersPrograms.FirstOrDefaultAsync(p => p.Id.Equals(userProgram.Id));

            if (entity == null)
            {
                return;
            }

            entity.UserId = userProgram.UserId;
            entity.ProgramId = userProgram.ProgramId;
            entity.StartedOn = userProgram.StartedOn;
            entity.FinishedOn = userProgram.FinishedOn;
            entity.Notes = userProgram.Notes;
            entity.ModifiedOn = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<int> AddClientMeasurementsAsync(UserMeasurements userMeasurements)
        {
            await _context.UsersMeasurements.AddAsync(userMeasurements);
            await _context.SaveChangesAsync();

            return userMeasurements.Id;
        }

        public async Task<UserMeasurements> GetClientMeasurementsAsync(int measurementsId)
        {
            return await _context.UsersMeasurements.FirstOrDefaultAsync(m => m.Id.Equals(measurementsId));
        }

        public async Task<List<UserMeasurements>> GetClientMeasurementsAsync(int? userId = null, int? userProgramId = null, string measurementsType = null)
        {
            IQueryable<UserMeasurements> query = _context.UsersMeasurements;

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId.Equals(userId.Value));
            }

            if (userProgramId.HasValue)
            {
                query = query.Where(p => p.UserProgramId.Equals(userProgramId.Value));
            }

            if (!string.IsNullOrEmpty(measurementsType))
            {
                query = query.Where(p => p.Type.Equals(measurementsType));
            }

            return await query.ToListAsync();
        }

        public async Task UpdateUserMeasurements(UserMeasurements userMeasurements)
        {
            var entity = await _context.UsersMeasurements.FirstOrDefaultAsync(p => p.Id.Equals(userMeasurements.Id));

            if (entity == null)
            {
                return;
            }

            entity.UserId = userMeasurements.UserId;
            entity.UserProgramId = userMeasurements.UserProgramId;
            entity.Type = userMeasurements.Type;
            entity.CreatedOn = userMeasurements.CreatedOn;
            entity.ModifiedOn = DateTime.Now;
            entity.Growth = userMeasurements.Growth;
            entity.Weight = userMeasurements.Weight;
            entity.Wrist = userMeasurements.Wrist;
            entity.Hand = userMeasurements.Hand;
            entity.Breast = userMeasurements.Breast;
            entity.WaistTop = userMeasurements.WaistTop;
            entity.WaistMiddle = userMeasurements.WaistMiddle;
            entity.WaistBottom = userMeasurements.WaistBottom;
            entity.Buttocks = userMeasurements.Buttocks;
            entity.Things = userMeasurements.Things;
            entity.Leg = userMeasurements.Leg;
            entity.KneeTop = userMeasurements.KneeTop;

            await _context.SaveChangesAsync();
        }



        public async Task<int> AddClientReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();

            return report.Id;
        }

        public async Task<Report> GetClientReportAsync(int reportId)
        {
            return await _context.Reports.FirstOrDefaultAsync(m => m.Id.Equals(reportId));
        }

        public async Task<List<Report>> GetClientReportsAsync(int? userId = null, int? userProgramId = null)
        {
            IQueryable<Report> query = _context.Reports;

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId.Equals(userId.Value));
            }

            if (userProgramId.HasValue)
            {
                query = query.Where(p => p.UserProgramId.Equals(userProgramId.Value));
            }

            return await query.ToListAsync();
        }
    }
}