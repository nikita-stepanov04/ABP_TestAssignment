using ABP_TestAssignment.Domain.Entities.Bookings;
using ABP_TestAssignment.Infrastructure.IRepositories.Bookings;
using Microsoft.EntityFrameworkCore;

namespace ABP_TestAssignment.Infrastructure.EFRepositories.Bookings
{
    public class EFBookingRepository : EFRepositoryBase<Booking>, IBookingRepository
    {
        public EFBookingRepository(EFDataContext context)
            : base(context) { }

        public Task<List<Booking>> GetAllBookingsForCompanyAsync(long companyID)
        {
            return DbSet
                .Include(b => b.Room)
                    .ThenInclude(r => r.AvailableServices)
                .Include(b => b.Services)
                .Where(b => b.CompanyID == companyID)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<bool> HasOverlapAsync(long roomID, DateTime start, DateTime end)
        {
            return DbSet.AnyAsync(b =>
                b.RoomID == roomID &&
                b.BookingStartTime < end &&
                b.BookingEndTime > start);
        }

        public Task<List<Booking>> GetByPeriodAsync(
           List<long> roomIDs,
           DateTime periodStart,
           DateTime periodEnd,
           CancellationToken cancellationToken = default)
        {
            return DbSet
                .Where(b => roomIDs.Contains(b.RoomID))
                .Where(b => b.BookingStartTime < periodEnd && b.BookingEndTime > periodStart)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
