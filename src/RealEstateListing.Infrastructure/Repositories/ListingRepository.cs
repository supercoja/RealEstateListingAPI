using RealEstateListing.Domain;
using RealEstateListing.Infrastructure.Data;
using RealEstateListingAPI.Services;

namespace RealEstateListing.Infrastructure.Repositories;

public class ListingRepository : GenericRepository<Listing>, IListingRepository
{ 
    public ListingRepository(ApplicationDbContext context) : base(context)
    {
    }
}              
