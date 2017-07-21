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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(@"User ID=postgres;Password=Tsunami9;Host=localhost;Port=5432;Database=hitfit.db;Pooling=true;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);

                entity.Property(e => e.MiddleName)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);
            });
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
