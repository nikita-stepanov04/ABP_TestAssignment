using System.ComponentModel.DataAnnotations;

namespace ABP_TestAssignment.Application.DTOs.Rooms
{
    public class AddRoomDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Room name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Room name must be between 3 and 50 characters.")]
        public string Name { get; set; } = null!;

        [Range(1, 1000, ErrorMessage = "Capacity must be at least 1 and not exceed 1000.")]
        public int Capacity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Base price per hour must be non-negative.")]
        public decimal BasePricePerHour { get; set; }
        public List<long>? AvailableServicesIDs { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AvailableServicesIDs != null)
            {
                if (AvailableServicesIDs.Any(id => id <= 0))
                    yield return new ValidationResult(
                        "Service ID must be a valid positive identifier",
                        new[] { nameof(AvailableServicesIDs) });

                if (AvailableServicesIDs.Distinct().Count() != AvailableServicesIDs.Count)
                    yield return new ValidationResult(
                        "Duplicate service IDs are not allowed",
                        new[] { nameof(AvailableServicesIDs) });
            }
        }
    }
}
