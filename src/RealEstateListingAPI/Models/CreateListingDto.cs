using System.ComponentModel.DataAnnotations;

namespace RealEstateListingApi.Models
{
    public class CreateListingDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(1, 100000000, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }                                                                                                                                          
    } 
    
    public class ListingDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public string? Description { get; set; }
    }
}