using Microsoft.EntityFrameworkCore;
using RealEstateListing.Domain;

namespace RealEstateListing.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }
    }
}
