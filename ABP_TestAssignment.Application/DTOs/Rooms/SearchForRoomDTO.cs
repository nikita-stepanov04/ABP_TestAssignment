using System.ComponentModel.DataAnnotations;

namespace ABP_TestAssignment.Application.DTOs.Rooms
{
    public class SearchForRoomDTO : IValidatableObject
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Capacity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime.HasValue && !EndTime.HasValue)
            {
                yield return new ValidationResult(
                    "End time is required when Start time is provided",
                    new[] { nameof(EndTime) });
            }

            if (EndTime.HasValue && !StartTime.HasValue)
            {
                yield return new ValidationResult(
                    "Start time is required when End time is provided",
                    new[] { nameof(StartTime) });
            }

            if (StartTime.HasValue && EndTime.HasValue && StartTime >= EndTime)
            {
                yield return new ValidationResult(
                    "Start time must be before End time",
                    new[] { nameof(StartTime), nameof(EndTime) });
            }
        }
    }
}
