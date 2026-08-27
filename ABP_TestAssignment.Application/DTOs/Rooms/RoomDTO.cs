using ABP_TestAssignment.Application.DTOs.Services;

namespace ABP_TestAssignment.Application.DTOs.Rooms
{
    public class RoomDTO
    {
        public long ID { get; set; }
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public decimal BasePricePerHour { get; set; }
        public List<ServiceDTO> AvailableServices { get; set; } = [];
    }
}
