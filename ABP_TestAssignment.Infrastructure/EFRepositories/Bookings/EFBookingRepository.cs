using ABP_TestAssignment.Domain.Entities.Bookings;
using ABP_TestAssignment.Infrastructure.IRepositories.Bookings;
using Microsoft.EntityFrameworkCore;

namespace ABP_TestAssignment.Infrastructure.EFRepositories.Bookings
{
    public class EFBookingRepository : EFRepositoryBase<Booking>, IBookingRepository
    {
        public EFBookingRepository(EFDataContext context)
            : base(context) { }

        public Task<bool> HasOverlapAsync(long roomID, DateTime start, DateTime end)
        {
            return DbSet.AnyAsync(b =>
                b.RoomID == roomID &&
                b.BookingStartTime < end &&
                b.BookingEndTime > start);
        }
    }
}
