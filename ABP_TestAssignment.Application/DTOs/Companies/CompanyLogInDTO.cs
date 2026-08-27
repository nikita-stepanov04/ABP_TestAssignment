using System.ComponentModel.DataAnnotations;

namespace ABP_TestAssignment.Application.DTOs.Companies
{
    public class CompanyLogInDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Email must be between 6 and 50 characters.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters.")]
        public string Password { get; set; } = null!;
    }
}
