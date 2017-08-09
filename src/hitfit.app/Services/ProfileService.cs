using hitfit.app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitfit.app.Services
{
    public class ProfileService
    {
        private readonly HitFitDbContext _context;

        public ProfileService(HitFitDbContext context)
        {
            _context = context;
        }


    }
}
