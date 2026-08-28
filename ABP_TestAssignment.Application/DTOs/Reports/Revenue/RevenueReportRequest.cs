using System.ComponentModel.DataAnnotations;

namespace ABP_TestAssignment.Application.DTOs.Reports.Revenue
{
    public class RevenueReportRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Start date is required")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateOnly EndDate { get; set; }

        public List<long>? RoomIDs { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
                yield return new ValidationResult(
                    "Start time must be before end time",
                    new[] { nameof(StartDate), nameof(EndDate) });

            if (RoomIDs?.Any(id => id <= 0) ?? false)
                yield return new ValidationResult(
                    "Service ID must be a valid positive identifier",
                    new[] { nameof(RoomIDs) });
        }
    }
}
