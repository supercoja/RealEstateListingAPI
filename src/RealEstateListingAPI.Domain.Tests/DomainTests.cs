using RealEstateListing.Domain;

namespace RealEstateListingAPI.Domain.Tests;

public class DomainTests
{
    public class ListingTests
    {
        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = "123";
            var title = "Test Title";
            var price = 100000;
            var description = "Test Description";

            // Act
            var listing = new Listing(id, title, price, description);

            // Assert
            Assert.Equal(id, listing.Id);
            Assert.Equal(title, listing.Title);
            Assert.Equal(price, listing.Price);
            Assert.Equal(description, listing.Description);
        }

        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly_WhenDescriptionIsNull()
        {
            // Arrange
            var id = "456";
            var title = "Another Test Title";
            var price = 200000;

            // Act
            var listing = new Listing(id, title, price, null);

            // Assert
            Assert.Equal(id, listing.Id);
            Assert.Equal(title, listing.Title);
            Assert.Equal(price, listing.Price);
            Assert.Null(listing.Description);
        }
}