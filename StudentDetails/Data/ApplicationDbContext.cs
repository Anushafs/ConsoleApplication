
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StudentDetails.Models;
using StudentDetails.Models.Entity;

namespace StudentDetails.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
         
            
        }
        public DbSet<Student> Students { get; set; }

        public DbSet<Users> UserDetails { get; set; }

        public DbSet<UserRole> UserRole { get; set; }
    }
}
