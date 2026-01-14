using RealEstateListing.Infrastructure.Data;
using RealEstateListingAPI.Services;

namespace RealEstateListing.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IListingRepository Listings { get; private set; }

    public UnitOfWork(ApplicationDbContext context, IListingRepository listingRepository)
    {
        _context = context;
        Listings = listingRepository;
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}