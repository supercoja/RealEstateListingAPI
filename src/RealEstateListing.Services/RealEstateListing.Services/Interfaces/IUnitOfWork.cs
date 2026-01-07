namespace RealEstateListing.Services.Interfaces;

public interface IUnitOfWork : IDisposable
{
        IListingRepository Listings { get; }
        Task<int> CompleteAsync();
}