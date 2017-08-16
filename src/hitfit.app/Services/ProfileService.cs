using hitfit.app.Models;
using hitfit.app.Converters;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace hitfit.app.Services
{
    public class ProfileService
    {
        private readonly HitFitDbContext _context;
        private readonly FileStorageImageService _imageService;
        private readonly MeasurementsManager _measurementsManager;

        public ProfileService(HitFitDbContext context, FileStorageImageService imageService, MeasurementsManager measurementsManager)
        {
            _context = context;
            _measurementsManager = measurementsManager;
        }

        public async Task<User> GetFrofileAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserMeasurements)
                .Include(u => u.UserPrograms)
                .ThenInclude(p => p.Program)
                //.Include("UserPrograms.Program")
                .FirstOrDefaultAsync();
        }

        public async Task UpdatePersonalDataAsync(int userId, byte[] photo, string firstName, string lastName, string phoneNumber, DateTime birthday, string notes)
        {
            var entity = await _context.FindAsync<User>(userId);

            if (entity == null)
            {
                throw new DbUpdateException("User not found", new Exception());
            }

            if (photo != null)
            {
                entity.Photo = photo;
            }
            
            entity.FirstName = firstName;
            entity.LastName = lastName;
            entity.PhoneNumber = phoneNumber;
            entity.Birthday = birthday;
            entity.Notes = notes;

            await _context.SaveChangesAsync();
        }

        public async Task<int> AddMeasurementsAsync(int userId, int userProgramId, List<IFormFile> images, string type, short growth, short weight, short wrist, short hand, short breast, short waistTop, short waistMiddle, short waistBottom, short buttocks, short thighs, short leg, short kneeTop)
        {
            var measurements = new UserMeasurements
            {
                UserId = userId,
                UserProgramId = userProgramId,
                Type = type,
                Growth = growth,
                Weight = weight,
                Wrist = wrist,
                Hand = hand,
                Breast = breast,
                WaistTop = waistTop,
                WaistMiddle = waistMiddle,
                WaistBottom = waistBottom,
                Buttocks = buttocks,
                Thighs = thighs,
                Leg = leg,
                KneeTop = kneeTop
            };

            var measurementsId = await _measurementsManager.AddMeasurementsAsync(measurements);

            List<UploadImage> uploadImages = new List<UploadImage>();
            foreach (var image in images)
            {
                var uploadImage = await FormFileConverter.ConvertToUploadImageAsync(image);

                uploadImages.Add(uploadImage);
            }

            await _imageService.SaveImagesToDiskAsync(userId, ImageRelationType.Measurements, measurementsId, uploadImages);

            return measurementsId;
        }

        public async Task<int> AddReport(int userId, int userProgramId, string type, List<byte[]> images, string description)
        {
            var entity = await _context.FindAsync<User>(userId);

            if (entity == null)
            {
                throw new DbUpdateException("User not found", new Exception());
            }

            var userProgram = await _context.UsersPrograms.FindAsync(userProgramId);

            if (userProgram == null)
            {
                throw new DbUpdateException("User program not found", new Exception());
            }

            var report = new Report
            {
                UserId = userId,
                UserProgramId = userProgramId,
                Type = type,
                Description = description
            };

            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();

            //_imageService.SaveImagesToDiskAsync()

            return report.Id;
        }

        public async Task<List<UserMeasurements>> GetMeasurements(int userId)
        {
            return await _context.UsersMeasurements.Where(m => m.UserId == userId).ToListAsync();
        }

        
    }
}
