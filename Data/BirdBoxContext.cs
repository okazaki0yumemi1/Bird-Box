using Bird_Box.Audio;
using Bird_Box.Authentication;
using Bird_Box.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Bird_Box.Data
{
    public class BirdBoxContext : DbContext
        {
        protected readonly IConfiguration Configuration;

        public BirdBoxContext(IConfiguration configuration)
                {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("SqliteResults"));
            //optionsBuilder.UseNpgsql(Configuration.GetConnectionString("ResultsDatabase"));
        }

        public DbSet<IdentifiedBird> BirdRecords { get; set; } = default!;
                public DbSet<Microphone> InputDevices { get; set; } = default!;
                public DbSet<ListeningTask> ListeningTasks { get; set; } = default!;
            }
}
