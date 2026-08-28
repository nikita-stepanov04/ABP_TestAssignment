using ABP_TestAssignment.Application.DTOs.Reports.Revenue;
using ABP_TestAssignment.Application.IBusinessServices.Reports;
using ABP_TestAssignment.Infrastructure.IRepositories.Bookings;

namespace ABP_TestAssignment.Application.BusinessServices.Reports
{
    public class RevenueReportBS : ReportBaseBS, IRevenueReportBS
    {
        private readonly IBookingRepository _bookingRepository;

        public RevenueReportBS(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<RevenueReportResponse> GenerateAsync(
            RevenueReportRequest request, CancellationToken cancellationToken = default)
        {
            var periodStart = DateTime.SpecifyKind(
                request.StartDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

            var periodEnd = DateTime.SpecifyKind(
                request.EndDate.AddDays(1).ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

            var bookings = await _bookingRepository.GetByPeriodAsync(
                request.RoomIDs,
                periodStart,
                periodEnd,
                cancellationToken);

            var bookingsByDay = bookings.ToLookup(b => DateOnly.FromDateTime(b.BookingStartTime));

            var dailyBreakdown = EnumerateDays(request.StartDate, request.EndDate).Select(day =>
            {
                var dayBookings = bookingsByDay[day];

                var roomRevenue = dayBookings.Sum(b => b.CalculatedTotalPrice);
                var serviceRevenue = dayBookings.Sum(b => b.Services.Sum(s => s.Price));

                return new DailyRevenue
                {
                    Date = day,
                    RoomRevenue = roomRevenue,
                    ServiceRevenue = serviceRevenue,
                    TotalRevenue = roomRevenue + serviceRevenue,
                    BookingsCount = dayBookings.Count()
                };
            }).ToList();

            return new RevenueReportResponse
            {
                PeriodStart = request.StartDate,
                PeriodEnd = request.EndDate,
                GeneratedAt = DateTime.UtcNow,
                DailyBreakdown = dailyBreakdown,
                TotalRoomRevenue = dailyBreakdown.Sum(d => d.RoomRevenue),
                TotalServiceRevenue = dailyBreakdown.Sum(d => d.ServiceRevenue),
                TotalRevenue = dailyBreakdown.Sum(d => d.TotalRevenue)
            };
        }        
    }
}
