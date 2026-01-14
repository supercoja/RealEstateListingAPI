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
        Id = id;
        Title = title;
        Price = price;
        Description = description;
    }
}