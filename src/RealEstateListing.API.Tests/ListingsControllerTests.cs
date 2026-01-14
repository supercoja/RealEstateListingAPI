using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RealEstateListing.Domain;
using System.Net;
using RealEstateListing.Common.Api;
using RealEstateListingApi.Controllers;
using RealEstateListingAPI.Services;

namespace RealEstateListing.API.Tests
{
    public class ListingsControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IListingRepository> _mockListingRepository;
        private readonly Mock<ILogger<ListingsController>> _mockLogger;
        private readonly ListingsController _controller;

        public ListingsControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockListingRepository = new Mock<IListingRepository>();
            _mockLogger = new Mock<ILogger<ListingsController>>();

            _mockUnitOfWork.Setup(uow => uow.Listings).Returns(_mockListingRepository.Object);

            _controller = new ListingsController(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllListings_WhenListingsExist_ReturnsOkObjectResultWithListingDtosInEnvelope()
        {
            // Arrange
            var listings = new List<Listing> { new("1", "123 Main St", 500000, null) };
            _mockListingRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(listings);

            // Act
            var actionResult = await _controller.GetAllListings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode); // Check status code

            // Assert that the value is an Envelope
            var envelope = Assert.IsType<Envelope<IEnumerable<RealEstateListingApi.Models.ListingDto>>>(okResult.Value);
            Assert.True(envelope.IsSuccess);
            Assert.Null(envelope.ErrorMessages);

            // Assert the actual data inside the envelope
            var actualDtos = Assert.IsAssignableFrom<IEnumerable<RealEstateListingApi.Models.ListingDto>>(envelope.Result);
            Assert.Single(actualDtos);
            Assert.Equal(listings.First().Id, actualDtos.First().Id);
        }

        [Fact]
        public async Task AddListing_WithValidData_ShouldReturnCreatedAtActionWithDtoInEnvelope()
        {
            // Arrange
            var createDto = new RealEstateListingApi.Models.CreateListingDto
            {
                Id = "valid-id",
                Title = "New House",
                Price = 300000m,
                Description = "A great house."
            };
            
            _mockListingRepository.Setup(repo => repo.GetByIdAsync(createDto.Id)).ReturnsAsync((Listing)null);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            // Act
            var actionResult = await _controller.AddListing(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);

            // Assert the value is an Envelope
            var envelope = Assert.IsType<Envelope<RealEstateListingApi.Models.ListingDto>>(createdResult.Value);
            Assert.True(envelope.IsSuccess);
            Assert.Null(envelope.ErrorMessages);

            // Assert the actual data inside the envelope
            var returnedDto = Assert.IsType<RealEstateListingApi.Models.ListingDto>(envelope.Result);
            Assert.Equal(createDto.Id, returnedDto.Id);

            _mockListingRepository.Verify(repo => repo.AddAsync(It.IsAny<Listing>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddListing_WithInvalidDomainData_ShouldReturnBadRequestWithErrorsInEnvelope()
        {
            // Arrange
            var invalidDto = new RealEstateListingApi.Models.CreateListingDto
            {
                Id = "invalid-id",
                Title = "House with bad price",
                Price = -100m, 
                Description = "This should fail."
            };

            // Act
            var actionResult = await _controller.AddListing(invalidDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);

            // Assert the value is an Envelope with errors
            var envelope = Assert.IsType<Envelope<object>>(badRequestResult.Value); 
            Assert.False(envelope.IsSuccess);
            Assert.NotNull(envelope.ErrorMessages);
            Assert.Contains("Price cannot be negative. (Parameter 'price')", envelope.ErrorMessages);

            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task GetAllListings_WhenNoListingsExist_ReturnsOkObjectResultWithEmptyListInEnvelope()
        {
            // Arrange
            _mockListingRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Listing>());

            // Act
            var actionResult = await _controller.GetAllListings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);

            var envelope = Assert.IsType<Envelope<IEnumerable<RealEstateListingApi.Models.ListingDto>>>(okResult.Value);
            Assert.True(envelope.IsSuccess);
            Assert.Empty(envelope.Result); 
        }
        
        [Fact]
        public async Task GetListingById_WhenListingExists_ReturnsOkObjectResultWithListingDtoInEnvelope()
        {
            // Arrange
            var listingId = "1";
            var listing = new Listing("1", "123 Main St", 500000, null);
            _mockListingRepository.Setup(repo => repo.GetByIdAsync(listingId)).ReturnsAsync(listing);

            // Act
            var actionResult = await _controller.GetListingById(listingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);

            var envelope = Assert.IsType<Envelope<RealEstateListingApi.Models.ListingDto>>(okResult.Value);
            Assert.True(envelope.IsSuccess);
            Assert.Null(envelope.ErrorMessages);
            Assert.Equal(listingId, envelope.Result.Id);
        }

        [Fact]
        public async Task GetListingById_WhenListingDoesNotExist_ReturnsNotFoundObjectResultWithErrorsInEnvelope()
        {
            // Arrange
            var listingId = "non-existent-id";
            _mockListingRepository.Setup(repo => repo.GetByIdAsync(listingId)).ReturnsAsync((Listing)null);

            // Act
            var actionResult = await _controller.GetListingById(listingId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);

            var envelope = Assert.IsType<Envelope<object>>(notFoundResult.Value);
            Assert.False(envelope.IsSuccess);
            Assert.NotNull(envelope.ErrorMessages);
            Assert.Contains($"Listing with ID {listingId} not found.", envelope.ErrorMessages);
        }

        [Fact]
        public async Task DeleteListing_WhenListingExists_ReturnsNoContentResult()
        {
            // Arrange
            var listingId = "1";
            var listing = new Listing(listingId, "123 Main St", 500000, null);
            _mockListingRepository.Setup(repo => repo.GetByIdAsync(listingId)).ReturnsAsync(listing);
            
            // Act
            var actionResult = await _controller.DeleteListing(listingId);

            // Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockListingRepository.Verify(repo => repo.Delete(listing), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteListing_WhenListingDoesNotExist_ReturnsNotFoundObjectResultWithErrorsInEnvelope()
        {
            // Arrange
            var listingId = "non-existent-id";
            _mockListingRepository.Setup(repo => repo.GetByIdAsync(listingId)).ReturnsAsync((Listing)null);

            // Act
            var actionResult = await _controller.DeleteListing(listingId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);

            var envelope = Assert.IsType<Envelope<object>>(notFoundResult.Value);
            Assert.False(envelope.IsSuccess);
            Assert.NotNull(envelope.ErrorMessages);
            Assert.Contains($"Listing with ID {listingId} not found.", envelope.ErrorMessages);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never); 
        }
    }
}