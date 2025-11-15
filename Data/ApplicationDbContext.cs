using group_12_ahmed_yu__GroupProject.Models;
using Microsoft.EntityFrameworkCore;

namespace group_12_ahmed_yu__GroupProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<group_12_ahmed_yu__GroupProject.Models.Patient> Patients { get; set; }
        public DbSet<group_12_ahmed_yu__GroupProject.Models.Doctor> Doctors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configurations can be added here if needed

            modelBuilder.Entity<Doctor>()
                .Property(d => d.ConsultationFee)
                .HasColumnType("decimal(18,2)");

        }
    }
}
