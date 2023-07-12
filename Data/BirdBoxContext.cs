using Microsoft.EntityFrameworkCore;

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
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("ResultsDatabase"));
        }
        public DbSet<Models.IdentifiedBird> BirdRecords { get; set; } = default!;
    }
}