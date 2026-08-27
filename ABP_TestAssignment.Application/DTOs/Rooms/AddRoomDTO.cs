using System.ComponentModel.DataAnnotations;

namespace ABP_TestAssignment.Application.DTOs.Rooms
{
    public class AddRoomDTO
    {
        [Required(ErrorMessage = "Room name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Room name must be between 3 and 50 characters.")]
        public string Name { get; set; } = null!;

        [Range(1, 1000, ErrorMessage = "Capacity must be at least 1 and not exceed 1000.")]
        public int Capacity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Base price per hour must be non-negative.")]
        public decimal BasePricePerHour { get; set; }
        public List<long>? AvailableServicesIDs { get; set; }
    }
}
