using Magnum_web_application.Models;
using Microsoft.EntityFrameworkCore;

namespace Magnum_web_application.Data
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set;}
        public DbSet<Fee> Fees { get; set; }
	}
}
