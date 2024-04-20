using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bird_Box.Authentication
{
    public class BirdBoxAuthContext : IdentityDbContext<ApplicationUser>
    {
        protected readonly IConfiguration Configuration;
        public BirdBoxAuthContext(DbContextOptions<BirdBoxAuthContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("AuthData"));
            //optionsBuilder.UseNpgsql(Configuration.GetConnectionString("ResultsDatabase"));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}