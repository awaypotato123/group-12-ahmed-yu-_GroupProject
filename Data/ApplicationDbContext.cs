using Microsoft.EntityFrameworkCore;

namespace group_12_ahmed_yu__GroupProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<group_12_ahmed_yu__GroupProject.Models.Patient> Patients { get; set; }
    }
}
