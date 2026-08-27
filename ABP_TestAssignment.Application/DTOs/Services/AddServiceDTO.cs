using System.ComponentModel.DataAnnotations;

namespace ABP_TestAssignment.Application.DTOs.Services
{
    public class AddServiceDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be non-negative")]
        public decimal Price { get; set; }
    }
}
