using ABP_TestAssignment.Domain.Entities.Bookings;

namespace ABP_TestAssignment.Infrastructure.IRepositories.Bookings
{
    public interface IBookingRepository : IRepositoryBase<Booking>
    {
        Task<bool> HasOverlapAsync(long roomID, DateTime start, DateTime end);
    }
}
