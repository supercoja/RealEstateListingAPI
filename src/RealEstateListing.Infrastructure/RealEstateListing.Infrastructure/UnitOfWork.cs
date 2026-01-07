using RealEstateListing.Infrastructure.Data;
using RealEstateListing.Infrastructure.Repositories;
using RealEstateListing.Services.Interfaces;

namespace RealEstateListing.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IListingRepository Listings { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Listings = new ListingRepository(_context);
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
}