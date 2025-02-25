using Microsoft.EntityFrameworkCore;
using Student_API.Models.Entities;

namespace Student_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // tables in the db
        public DbSet<Student> Students { get; set; }
        public DbSet<Address> Address { get; set; }
    }
}
