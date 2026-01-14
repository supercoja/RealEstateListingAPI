using Microsoft.AspNetCore.Mvc;
using RealEstateListingApi.Mapping;
using RealEstateListingApi.Models;
using RealEstateListingAPI.Services;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ListingDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ListingDto>>> GetAllListings()
        {
            try
            {
                var listings = await _unitOfWork.Listings.GetAllAsync();
                var listingsDto = listings.Select(l => l.ToDto());
                return Ok(listingsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all listings.");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }

        // GET: api/Listings/{id}
        [HttpGet("{id}")]
        [Tags("Listings Retrieval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListingDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListingDto>> GetListingById(string id)
        {
            try
            {
                var listing = await _unitOfWork.Listings.GetByIdAsync(id);

                if (listing == null)
                {
                    return NotFound($"Listing with ID {id} not found.");
                }

                return Ok(listing.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving listing with ID {ListingId}.", id);
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }

        // POST: api/Listings
        [HttpPost]
        [Tags("Listings Management")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ListingDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListingDto>> AddListing([FromBody] CreateListingDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var exits = await _unitOfWork.Listings.GetByIdAsync(createDto.Id);
                if (exits != null)
                {
                    return BadRequest($"Listing with ID {createDto.Id} already exists.");
                }

                var newListing = createDto.ToEntity();
                
                await _unitOfWork.Listings.AddAsync(newListing);
                await _unitOfWork.CompleteAsync();

                var createdListingDto = newListing.ToDto();

                return CreatedAtAction(nameof(GetListingById), new { id = createdListingDto.Id }, createdListingDto);
            }
            catch (ArgumentException ex) 
            {
                _logger.LogWarning(ex, "Domain validation failed for new listing: {ErrorMessage}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while creating a new listing.");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        
        // DELETE: api/Listings/{id}
        [HttpDelete("{id}")]
        [Tags("Listings Management")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteListing(string id)
        {
            try
            {
                var listing = await _unitOfWork.Listings.GetByIdAsync(id);

                if (listing == null)
                {
                    return NotFound($"Listing with ID {id} not found.");
                }

                _unitOfWork.Listings.Delete(listing);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting listing with ID {ListingId}.", id);
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
    }
}
