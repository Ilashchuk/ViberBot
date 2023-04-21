using Microsoft.EntityFrameworkCore;
using ViberApp.Controllers;

namespace ViberApp.Models
{
    public class ViberDbContext : DbContext
    {
        public DbSet<TrackLocation> TrackLocation { get; set; }
        public DbSet<TotalWalk> TotalWalks { get; set; }
        public DbSet<ModelForTop10> ModelForTop10s { get; set; }
        public ViberDbContext(DbContextOptions<ViberDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        

    }
}
