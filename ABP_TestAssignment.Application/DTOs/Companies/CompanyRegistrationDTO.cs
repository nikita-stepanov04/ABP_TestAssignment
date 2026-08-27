using ABP_TestAssignment.Domain.Entities.Companies;
using System.ComponentModel.DataAnnotations;

namespace ABP_TestAssignment.Application.DTOs.Companies
{
    public class CompanyRegistrationDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Email must be between 6 and 50 characters.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters.")]
        public string Password { get; set; } = null!;

        [StringLength(50, MinimumLength = 6, ErrorMessage = "Master password must be between 6 and 50 characters.")]
        public string? MasterPassword { get; set; }

        public Role Role { get; set; } = Role.User;
    }

}
