using MAPay.Models;
using Microsoft.EntityFrameworkCore;

namespace MAPay.Data
{
    public class MAPayAPIDbContext : DbContext
    {
        public MAPayAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AdminSignUp> Admin { get; set; }
        public DbSet<UserSignUp> User { get; set; }
        public DbSet<DocumentUpload> Document { get; set; }
        public DbSet<DoctorSignUp> Doctor { get; set; }
    }
}
