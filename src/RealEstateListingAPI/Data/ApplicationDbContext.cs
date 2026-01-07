using Microsoft.EntityFrameworkCore;
using RealEstateListing.Domain;
using RealEstateListingApi.Models;

namespace RealEstateListingApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }
    }
}
