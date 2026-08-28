using System.ComponentModel.DataAnnotations;

namespace ABP_TestAssignment.Application.DTOs.Bookings
{    
    public class BookRoomDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Room ID is required")]
        public long RoomID { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        public DateTime EndTime { get; set; }

        public List<long> SelectedServicesIDs { get; set; } = [];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            TimeSpan WorkingDayStart = TimeSpan.FromHours(6);
            TimeSpan WorkingDayEnd = TimeSpan.FromHours(23);

            if (RoomID <= 0)
                yield return new ValidationResult(
                    "Room ID must be a positive number",
                    new[] { nameof(RoomID) });

            if (StartTime >= EndTime)
                yield return new ValidationResult(
                    "Start time must be before end time",
                    new[] { nameof(StartTime), nameof(EndTime) });

            if (StartTime < DateTime.UtcNow)
                yield return new ValidationResult(
                    "Cannot book a room in the past",
                    new[] { nameof(StartTime) });

            if (SelectedServicesIDs.Any(id => id <= 0))
                yield return new ValidationResult(
                    "Service ID must be a valid positive identifier",
                    new[] { nameof(SelectedServicesIDs) });

            if (SelectedServicesIDs.Distinct().Count() != SelectedServicesIDs.Count)
                yield return new ValidationResult(
                    "Duplicate service IDs are not allowed",
                    new[] { nameof(SelectedServicesIDs) });

            
            if (StartTime.Date != EndTime.Date)
            {
                yield return new ValidationResult(
                    "Booking cannot span across midnight — start and end must be on the same day",
                    new[] { nameof(StartTime), nameof(EndTime) });
                yield break;
            }

            if (StartTime.TimeOfDay < WorkingDayStart || StartTime.TimeOfDay >= WorkingDayEnd)
            {
                yield return new ValidationResult(
                    $"Booking start must be between {WorkingDayStart:hh\\:mm} and {WorkingDayEnd:hh\\:mm}",
                    new[] { nameof(StartTime) });
            }

            if (EndTime.TimeOfDay <= WorkingDayStart || EndTime.TimeOfDay > WorkingDayEnd)
            {
                yield return new ValidationResult(
                    $"Booking end must be between {WorkingDayStart:hh\\:mm} and {WorkingDayEnd:hh\\:mm}",
                    new[] { nameof(EndTime) });
            }
        }
    }
}
