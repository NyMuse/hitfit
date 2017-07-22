using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace hitfit.api.Models
{
    public class HitFitDbContext : DbContext
    {
        public HitFitDbContext(DbContextOptions<HitFitDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<UserMeasurements>()
                .HasOne(u => u.User)
                .WithMany(um => um.UserMeasurements)
                .HasForeignKey(um => um.UserId);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserMeasurements> UserMeasurements { get; set; }
    }
}
