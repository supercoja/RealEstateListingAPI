using RealEstateListing.Domain;
using RealEstateListingApi.Models;

namespace RealEstateListingApi.Mapping;

public static class MappingExtensions
{
    public static ListingDto ToDto(this Listing listing)
    {
        return new ListingDto
        {
            Id = listing.Id,
            Title = listing.Title,
            Price = listing.Price,
            Description = listing.Description
        };
    }

    public static Listing ToEntity(this CreateListingDto createDto)
    {
        return new Listing(createDto.Id, createDto.Title, createDto.Price, createDto.Description);
    }                                                                              
}                          