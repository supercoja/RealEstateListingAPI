using Microsoft.AspNetCore.Mvc;
using RealEstateListingApi.Models;
using RealEstateListing.Services.Interfaces;
using RealEstateListingApi.Mapping;

namespace RealEstateListingApi.Controllers
{
    [ApiController]
    [Route("api/Listings")]
    public class ListingsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // GET: api/Listings
        [HttpGet]
        [Tags("Listings Retrieval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ListingDto>))]
        public async Task<ActionResult<IEnumerable<ListingDto>>> GetAllListings()
        {
            var listings = await _unitOfWork.Listings.GetAllAsync();
            var listingsDto = listings.Select(l => l.ToDto());
            return Ok(listingsDto);
        }

        // GET: api/Listings/{id}
        [HttpGet("{id}")]
        [Tags("Listings Retrieval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListingDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListingDto>> GetListingById(string id)
        {
            var listing = await _unitOfWork.Listings.GetByIdAsync(id);

            if (listing == null)
            {
                return NotFound($"Listing with ID {id} not found.");
            }

            return Ok(listing.ToDto());
        }

        // POST: api/Listings
        [HttpPost]
        [Tags("Listings Management")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ListingDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListingDto>> AddListing([FromBody] CreateListingDto createDto)
        {
            var exits = await _unitOfWork.Listings.GetByIdAsync(createDto.Id);
            if (exits != null)
            {
                return BadRequest($"Listing with ID {createDto.Id} already exists.");
            }
            else
            {
                var newListing = createDto.ToEntity();

                await _unitOfWork.Listings.AddAsync(newListing);
                await _unitOfWork.CompleteAsync();

                var createdListingDto = newListing.ToDto();

                return CreatedAtAction(nameof(GetListingById), new { id = createdListingDto.Id }, createdListingDto);
            }
        }
        
        // DELETE: api/Listings/{id}
        [HttpDelete("{id}")]
        [Tags("Listings Management")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteListing(string id)
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
    }
}

