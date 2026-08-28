using ABP_TestAssignment.Domain.Entities.Bookings;

namespace ABP_TestAssignment.Infrastructure.IRepositories.Bookings
{
    public interface IBookingRepository : IRepositoryBase<Booking>
    {
        Task<bool> HasOverlapAsync(long roomID, DateTime start, DateTime end);
        Task<List<Booking>> GetAllBookingsForCompanyAsync(long companyID);
        Task<List<Booking>> GetByPeriodAsync(
            List<long>? roomIds,
            DateTime periodStart,
            DateTime periodEnd,
            CancellationToken cancellationToken = default);
    }
}
