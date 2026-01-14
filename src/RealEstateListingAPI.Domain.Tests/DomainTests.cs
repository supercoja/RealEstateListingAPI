using RealEstateListing.Domain;

namespace RealEstateListingAPI.Domain.Tests;

public class DomainTests
{
    public class ListingTests
    {
        [Fact]
        public void CreateListing_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var id = "valid-id";
            var title = "Beautiful House";
            var price = 250000m;
            var description = "A beautiful house with a garden.";

            // Act
            var listing = new Listing(id, title, price, description);

            // Assert
            Assert.NotNull(listing);
            Assert.Equal(id, listing.Id);
            Assert.Equal(title, listing.Title);
            Assert.Equal(price, listing.Price);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CreateListing_WithInvalidId_ShouldThrowArgumentException(string invalidId)
        {
            // Arrange
            var title = "Beautiful House";
            var price = 250000m;
            var description = "A beautiful house with a garden.";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Listing(invalidId, title, price, description));
            
            Assert.Equal("id", exception.ParamName);
        }

        [Fact]
        public void CreateListing_WithNegativePrice_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var id = "valid-id";
            var title = "Beautiful House";
            var negativePrice = -100m;
            var description = "A beautiful house with a garden.";

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Listing(id, title, negativePrice, description));

            Assert.Equal("price", exception.ParamName);
        }
    }
}