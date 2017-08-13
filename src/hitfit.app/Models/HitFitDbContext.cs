using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models.Dictionaries;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hitfit.app.Models
{
    public class HitFitDbContext : IdentityDbContext<User, Role, int>
    {
        public HitFitDbContext(DbContextOptions<HitFitDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<User>()
                .Ignore(c => c.Roles)
                .Ignore(c => c.TwoFactorEnabled)
                .Ignore(c => c.ConcurrencyStamp)
                //.Ignore(c => c.NormalizedEmail)
                .Ignore(c => c.SecurityStamp)
                //.Ignore(c => c.NormalizedUserName)
                .Ignore(c => c.Claims);

            modelBuilder.Entity<IdentityUserRole<int>>()
                .HasKey(c => new {c.UserId, c.RoleId});

            modelBuilder.Entity<Role>()
                .Ignore(c => c.Claims)
                .Ignore(c => c.ConcurrencyStamp);

            modelBuilder.Ignore<IdentityUserLogin<int>>();
            //modelBuilder.Ignore<IdentityUserRole<int>>();
            modelBuilder.Ignore<IdentityUserToken<int>>();

            modelBuilder.Entity<UserDetails>()
                .HasOne(u => u.User)
                .WithOne(d => d.Details)
                .HasForeignKey<UserDetails>(u => u.UserId);

            modelBuilder.Entity<UserProgram>()
                .HasOne(u => u.User)
                .WithMany(p => p.UserPrograms)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserProgram>()
                .HasOne(p => p.Program)
                .WithMany(p => p.UserPrograms)
                .HasForeignKey(p => p.ProgramId);

            modelBuilder.Entity<UserMeasurements>()
                .HasOne(u => u.User)
                .WithMany(um => um.UserMeasurements)
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<Report>()
                .HasOne(u => u.User)
                .WithMany(r => r.Reports)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Report>()
                .HasOne(u => u.UserProgram)
                .WithMany(r => r.Reports)
                .HasForeignKey(r => r.UserProgramId);

            modelBuilder.Entity<Article>()
                .HasOne(u => u.Author)
                .WithMany(a => a.Articles)
                .HasForeignKey(a => a.AuthorId);

            modelBuilder.Entity<Article>()
                .HasOne(u => u.Category)
                .WithMany(a => a.Articles)
                .HasForeignKey(u => u.CategoryId);
        }

        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<UserDetails> UsersDetails { get; set; }
        public virtual DbSet<UserMeasurements> UsersMeasurements { get; set; }
        public virtual DbSet<UserProgram> UsersPrograms { get; set; }
        public virtual DbSet<ImageObject> Images { get; set; }
        public virtual DbSet<Article> Articles { get; set; }

        public virtual DbSet<MeasurementType> MeasurementTypes { get; set; }
        public virtual DbSet<ProgramType> ProgramTypes { get; set; }
        public virtual DbSet<ReportType> ReportTypes { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }
    }
}
