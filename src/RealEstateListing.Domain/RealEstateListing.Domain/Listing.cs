namespace RealEstateListing.Domain;

public class Listing
{
    public string Id { get; private set; } = string.Empty;  // Default to empty string if nulls aren't allowed
    public string Title { get; private set; } = string.Empty;
    public decimal Price { get; private set; }  // Decimal is a value type and non-nullable by default
    public string? Description { get; private set; }  // Mark as nullable if appropriate
}