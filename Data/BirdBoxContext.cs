using Bird_Box.Audio;
using Bird_Box.Models;
using Bird_Box.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Bird_Box.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BirdBoxContext : DbContext
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected readonly IConfiguration Configuration;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public BirdBoxContext(IConfiguration configuration)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Configuration = configuration;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("SqliteResults"));
            //optionsBuilder.UseNpgsql(Configuration.GetConnectionString("ResultsDatabase"));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public DbSet<IdentifiedBird> BirdRecords { get; set; } = default!;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public DbSet<Microphone> InputDevices { get; set; } = default!;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
