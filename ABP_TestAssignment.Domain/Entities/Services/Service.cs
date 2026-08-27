using ABP_TestAssignment.Domain.Entities.Rooms;

namespace ABP_TestAssignment.Domain.Entities.Services
{
    public class Service : EntityBase
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public List<Room> Rooms { get; set; } = []; 
    }
}
