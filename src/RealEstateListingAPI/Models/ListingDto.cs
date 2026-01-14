namespace RealEstateListingApi.Models;

public class ListingDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; } 
    public string? Description { get; set; }
}
