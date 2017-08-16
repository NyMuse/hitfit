using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models;
using Microsoft.EntityFrameworkCore;

namespace hitfit.app.Managers
{
    public class MeasurementsManager
    {
        private readonly HitFitDbContext _context;

        public MeasurementsManager(HitFitDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddMeasurementsAsync(UserMeasurements measurements)
        {
            int measurementsId;

            if (await _context.FindAsync<User>(measurements.UserId) == null)
            {
                throw new Exception("User not found");
            }

            if (await _context.UsersPrograms.FindAsync(measurements.UserProgramId) == null)
            {
                throw new Exception("User program not found");
            }

            if (await _context.MeasurementTypes.FirstOrDefaultAsync(t => t.Key == measurements.Type) == null)
            {
                throw new Exception("Measurements type not found");
            }

            var measurementsEntity =
                await _context.UsersMeasurements.FirstOrDefaultAsync(
                    m => m.UserId == measurements.UserId && m.UserProgramId == measurements.UserProgramId);

            if (measurementsEntity == null)
            {
                await _context.UsersMeasurements.AddAsync(measurements);

                measurementsId = measurements.Id;
            }
            else
            {
                //measurementsEntity.UserId = measurements.UserId;
                //measurementsEntity.UserProgramId = measurements.UserProgramId;
                //measurementsEntity.Type = measurements.Type;
                measurementsEntity.Growth = measurements.Growth;
                measurementsEntity.Weight = measurements.Weight;
                measurementsEntity.Wrist = measurements.Wrist;
                measurementsEntity.Hand = measurements.Hand;
                measurementsEntity.Breast = measurements.Breast;
                measurementsEntity.WaistTop = measurements.WaistTop;
                measurementsEntity.WaistMiddle = measurements.WaistMiddle;
                measurementsEntity.WaistBottom = measurements.WaistBottom;
                measurementsEntity.Buttocks = measurements.Buttocks;
                measurementsEntity.Thighs = measurements.Thighs;
                measurementsEntity.Leg = measurements.Leg;
                measurementsEntity.KneeTop = measurements.KneeTop;

                measurementsId = measurementsEntity.Id;
            }

            await _context.SaveChangesAsync();

            return measurementsId;
        }

        public async Task<UserMeasurements> GetMeasurementsAsync(int measurementsId)
        {
            return await _context.UsersMeasurements.FindAsync(measurementsId);
        }

        public async Task<List<UserMeasurements>> GetMeasurementsAsync(int? userId = null, int? userProgramId = null, string type = null)
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

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.Type.Equals(type));
            }

            return await query.ToListAsync();
        }

        //public async Task UpdateMeasurements(UserMeasurements userMeasurements)
        //{
        //    var entity = await _context.UsersMeasurements.FirstOrDefaultAsync(p => p.Id.Equals(userMeasurements.Id));

        //    if (entity == null)
        //    {
        //        return;
        //    }

        //    entity.UserId = userMeasurements.UserId;
        //    entity.UserProgramId = userMeasurements.UserProgramId;
        //    entity.Type = userMeasurements.Type;
        //    entity.Growth = userMeasurements.Growth;
        //    entity.Weight = userMeasurements.Weight;
        //    entity.Wrist = userMeasurements.Wrist;
        //    entity.Hand = userMeasurements.Hand;
        //    entity.Breast = userMeasurements.Breast;
        //    entity.WaistTop = userMeasurements.WaistTop;
        //    entity.WaistMiddle = userMeasurements.WaistMiddle;
        //    entity.WaistBottom = userMeasurements.WaistBottom;
        //    entity.Buttocks = userMeasurements.Buttocks;
        //    entity.Thighs = userMeasurements.Thighs;
        //    entity.Leg = userMeasurements.Leg;
        //    entity.KneeTop = userMeasurements.KneeTop;

        //    await _context.SaveChangesAsync();
        //}

    }
}
