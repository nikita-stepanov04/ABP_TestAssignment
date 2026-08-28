using ABP_TestAssignment.Application.DTOs.Rooms;
using ABP_TestAssignment.Application.DTOs.Services;

namespace ABP_TestAssignment.Application.DTOs.Bookings
{
    public class BookingDTO
    {
        public long ID { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartTime { get; set; }
        public DateTime BookingEndTime { get; set; }
        public decimal CalculatedTotalPrice { get; set; }
        public RoomDTO Room { get; set; } = null!;
        public List<ServiceDTO> Services { get; set; } = [];
    }
}
