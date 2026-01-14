namespace RealEstateListing.Domain;
public class Listing
{
    public string Id { get; private set; }
    public string Title { get; internal set; }
    public decimal Price { get; internal set; }
    public string? Description { get; internal set; }

    private Listing() { }
    
    public Listing(string id, string title, decimal price, string? description)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Listing ID cannot be empty or null.", nameof(id));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
        }

        Id = id;
        Title = title;
        Price = price;
        Description = description;
    }
}