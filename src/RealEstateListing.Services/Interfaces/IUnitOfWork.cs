namespace RealEstateListingAPI.Services;

public interface IUnitOfWork : IDisposable
{
    IListingRepository Listings { get; }
    Task<int> CompleteAsync();
}