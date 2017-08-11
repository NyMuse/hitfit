using hitfit.app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace hitfit.app.Services
{
    public class ProfileService
    {
        private readonly HitFitDbContext _context;

        public ProfileService(HitFitDbContext context)
        {
            _context = context;
        }

        //public async Task<User> GetFullFrofile(int userId)
        //{
        //    //var user _context.Users
        //    //    .Include(u => u.Details)
        //    //    .Include(u => u.UserMeasurements)
        //}

        public async Task<List<UserMeasurements>> GetMeasurements(int userId)
        {
            return await _context.UsersMeasurements.Where(m => m.UserId == userId).ToListAsync();
        }
    }
}
