using ABP_TestAssignment.Domain.Entities.Services;

namespace ABP_TestAssignment.Domain.Entities.Rooms
{
    public class Room : EntityBase
    {
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public decimal BasePricePerHour { get; set; }
        public List<Service> AvailableServices { get; set; } = [];
    }
}
