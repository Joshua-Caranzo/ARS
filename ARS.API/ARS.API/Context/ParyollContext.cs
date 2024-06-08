using Ars.API.Models;
using ARS.API.Models;
using Microsoft.EntityFrameworkCore;
using Payroll.API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Payroll.API.Context
{
    public class ParyollContext(DbContextOptions<ParyollContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<GradeLevel> GradeLevels { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Strand> Strands { get; set; }
        public DbSet<SchoolSection> SchoolSections { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<EnrollStudent> EnrollStudents { get; set; }
        public DbSet<StudentSection> StudentSections { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }

        //to Delete
 
        public DbSet<TimePolicy> TimePolicies { get; set; }
        public DbSet<HrisShift> hrisShifts { get; set; }
        public DbSet<DeviceIdMap> DeviceIdMaps { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<LogTranstype> LogTranstypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<AppSetting>().ToTable("AppSettings", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<School>().ToTable("School", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<GradeLevel>().ToTable("GradeLevel", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Track>().ToTable("Track", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Strand>().ToTable("Strand", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<SchoolSection>().ToTable("SchoolSection", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Student>().ToTable("Student", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<SchoolYear>().ToTable("SchoolYear", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Semester>().ToTable("Semester", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<EnrollStudent>().ToTable("EnrollStudent", t => t.ExcludeFromMigrations());

            modelBuilder.Entity<TimePolicy>().ToTable("hrisTIME_POLICIES", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<HrisShift>().ToTable("hrisSHIFTS", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<DeviceIdMap>().ToTable("hrisDTR_DEVICEIDMAP", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Station>().ToTable("hrisDTR_STATION", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<LogTranstype>().ToTable("hrisDTR_LOG_TRANSTYPE", t => t.ExcludeFromMigrations());
        }
    }
}
