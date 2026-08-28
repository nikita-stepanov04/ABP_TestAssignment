using ABP_TestAssignment.Domain.Entities.Companies;
using ABP_TestAssignment.Domain.Entities.Rooms;
using ABP_TestAssignment.Domain.Entities.Services;

namespace ABP_TestAssignment.Domain.Entities.Bookings
{
    public class Booking : EntityBase
    {
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartTime { get; set; }
        public DateTime BookingEndTime { get; set; }
        public decimal CalculatedTotalPrice { get; set; }

        public long CompanyID { get; set; }
        public Company? Company { get; set; }

        public long RoomID { get; set; }
        public Room? Room { get; set; }

        public List<Service> Services { get; set; } = [];
    }
}
