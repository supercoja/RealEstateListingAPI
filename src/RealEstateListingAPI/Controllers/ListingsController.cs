using Microsoft.AspNetCore.Mvc;
using RealEstateListing.Common.Api;
using RealEstateListingApi.Mapping;
using RealEstateListingApi.Models;
using RealEstateListingAPI.Services;
using System.Net;

namespace RealEstateListingApi.Controllers
{
    [ApiController]
    [Route("api/Listings")]
    public class ListingsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ListingsController> _logger;

        public ListingsController(IUnitOfWork unitOfWork, ILogger<ListingsController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Listings
        [HttpGet]
        [Tags("Listings Retrieval")]
        [ProducesResponseType(typeof(Envelope<IEnumerable<ListingDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Envelope<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Envelope<IEnumerable<ListingDto>>>> GetAllListings()
        {
            try
            {
                var listings = await _unitOfWork.Listings.GetAllAsync();
                var listingsDto = listings.Select(l => l.ToDto());
                return Ok(Envelope.Ok(listingsDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all listings.");
                return StatusCode((int)HttpStatusCode.InternalServerError, Envelope.Error("An internal server error occurred. Please try again later."));
            }
        }

        // GET: api/Listings/{id}
        [HttpGet("{id}")]
        [Tags("Listings Retrieval")]
        [ProducesResponseType(typeof(Envelope<ListingDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Envelope<object>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Envelope<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Envelope<ListingDto>>> GetListingById(string id)
        {
            try
            {
                var listing = await _unitOfWork.Listings.GetByIdAsync(id);

                if (listing == null)
                {
                    return NotFound(Envelope.Error($"Listing with ID {id} not found."));
                }

                return Ok(Envelope.Ok(listing.ToDto()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving listing with ID {ListingId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, Envelope.Error("An internal server error occurred. Please try again later."));
            }
        }

        // POST: api/Listings
        [HttpPost]
        [Tags("Listings Management")]
        [ProducesResponseType(typeof(Envelope<ListingDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Envelope<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Envelope<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Envelope<ListingDto>>> AddListing([FromBody] CreateListingDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(Envelope.Error(errorMessages));
            }
            
            try
            {
                var exits = await _unitOfWork.Listings.GetByIdAsync(createDto.Id);
                if (exits != null)
                {
                    return BadRequest(Envelope.Error($"Listing with ID {createDto.Id} already exists."));
                }

                var newListing = createDto.ToEntity();
                
                await _unitOfWork.Listings.AddAsync(newListing);
                await _unitOfWork.CompleteAsync();

                var createdListingDto = newListing.ToDto();

                return CreatedAtAction(nameof(GetListingById), new { id = createdListingDto.Id }, Envelope.Ok(createdListingDto));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed for new listing: {ErrorMessage}", ex.Message);
                return BadRequest(Envelope.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new listing.");
                return StatusCode((int)HttpStatusCode.InternalServerError, Envelope.Error("An internal server error occurred. Please try again later."));
            }
        }
        
        // DELETE: api/Listings/{id}
        [HttpDelete("{id}")]
        [Tags("Listings Management")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)] 
        [ProducesResponseType(typeof(Envelope<object>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Envelope<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteListing(string id)
        {
            try
            {
                var listing = await _unitOfWork.Listings.GetByIdAsync(id);

                if (listing == null)
                {
                    return NotFound(Envelope.Error($"Listing with ID {id} not found."));
                }

                _unitOfWork.Listings.Delete(listing);
                await _unitOfWork.CompleteAsync();

                return NoContent(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting listing with ID {ListingId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, Envelope.Error("An internal server error occurred. Please try again later."));
            }
        }
    }
}
