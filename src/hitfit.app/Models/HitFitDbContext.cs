using hitfit.app.Models.Dictionaries;
using Microsoft.AspNetCore.Identity;
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

            modelBuilder.Entity<User>().ForNpgsqlToTable("users")
                .Ignore(c => c.TwoFactorEnabled)
                .Ignore(c => c.ConcurrencyStamp)
                .Ignore(c => c.SecurityStamp)
                .Ignore(c => c.Claims);

            modelBuilder.Entity<IdentityUserRole<int>>().ForNpgsqlToTable("userroles")
                .HasKey(c => new {c.UserId, c.RoleId});

            modelBuilder.Entity<IdentityUserRole<int>>().Property(p => p.UserId).ForNpgsqlHasColumnName("userid");
            modelBuilder.Entity<IdentityUserRole<int>>().Property(p => p.RoleId).ForNpgsqlHasColumnName("roleid");

            modelBuilder.Entity<Role>().ForNpgsqlToTable("roles")
                .Ignore(c => c.Claims)
                .Ignore(c => c.ConcurrencyStamp);

            modelBuilder.Ignore<IdentityUserLogin<int>>();
            modelBuilder.Ignore<IdentityUserToken<int>>();

            modelBuilder.Entity<UserProgram>().ForNpgsqlToTable("userprograms");

            modelBuilder.Entity<UserProgram>()
                .HasOne(u => u.User)
                .WithMany(p => p.UserPrograms)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserProgram>()
                .HasOne(p => p.Program)
                .WithMany(p => p.UserPrograms)
                .HasForeignKey(p => p.ProgramId);

            modelBuilder.Entity<UserMeasurements>().ForNpgsqlToTable("usermeasurements")
                .HasOne(u => u.User)
                .WithMany(um => um.UserMeasurements)
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<Program>().ForNpgsqlToTable("programs");
            modelBuilder.Entity<Report>().ForNpgsqlToTable("reports");
            modelBuilder.Entity<Article>().ForNpgsqlToTable("articles");

            modelBuilder.Entity<Report>()
                .HasOne(u => u.User)
                .WithMany(r => r.Reports)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Report>()
                .HasOne(u => u.UserProgram)
                .WithMany(r => r.Reports)
                .HasForeignKey(r => r.UserProgramId);
        }

        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<UserMeasurements> UsersMeasurements { get; set; }
        public virtual DbSet<UserProgram> UsersPrograms { get; set; }
        public virtual DbSet<Article> Articles { get; set; }

        public virtual DbSet<MeasurementType> MeasurementTypes { get; set; }
        public virtual DbSet<ProgramType> ProgramTypes { get; set; }
        public virtual DbSet<ReportType> ReportTypes { get; set; }
    }
}
