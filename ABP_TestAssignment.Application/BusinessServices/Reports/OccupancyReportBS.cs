using ABP_TestAssignment.Application.BusinessServices.Bookings;
using ABP_TestAssignment.Application.DTOs.Reports.Occupancy;
using ABP_TestAssignment.Application.IBusinessServices.Reports;
using ABP_TestAssignment.Domain.Entities.Bookings;
using ABP_TestAssignment.Domain.Entities.Rooms;
using ABP_TestAssignment.Infrastructure.IRepositories.Bookings;
using ABP_TestAssignment.Infrastructure.IRepositories.Rooms;

namespace ABP_TestAssignment.Application.BusinessServices.Reports
{
    public class OccupancyReportBS : IOccupancyReportBS
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        private static readonly decimal DailyAvailableHours =
            (decimal)(BusinessHours.WorkingDayEnd - BusinessHours.WorkingDayStart).TotalHours;

        public OccupancyReportBS(IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<OccupancyReportResponse> GenerateAsync(
            OccupancyReportRequest request, CancellationToken cancellationToken = default)
        {
            var rooms = request.RoomIDs == null
                ? await _roomRepository.GetAllRoomsAsync()
                : await _roomRepository.GetByIdsAsync(request.RoomIDs, cancellationToken);

            var periodStart = DateTime.SpecifyKind(
                request.StartDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

            var periodEnd = DateTime.SpecifyKind(
                request.EndDate!.AddDays(1).ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

            var bookings = await _bookingRepository.GetByPeriodAsync(
                rooms.Select(r => r.ID).ToList(),
                periodStart,
                periodEnd,
                cancellationToken
            );

            var bookingsByRoom = bookings.ToLookup(b => b.RoomID);

            var roomOccupancies = rooms
                .Select(room => BuildRoomOccupancy(room, bookingsByRoom[room.ID], request.StartDate, request.EndDate))
                .ToList();

            return new OccupancyReportResponse
            {
                PeriodStart = request.StartDate,
                PeriodEnd = request.EndDate,
                GeneratedAt = DateTime.UtcNow,
                Rooms = roomOccupancies,
                Summary = BuildSummary(roomOccupancies)
            };
        }

        private RoomOccupancy BuildRoomOccupancy(
            Room room, 
            IEnumerable<Booking> roomBookings, 
            DateOnly periodStart, 
            DateOnly periodEnd)
        {
            var bookingsByDay = roomBookings.ToLookup(b => DateOnly.FromDateTime(b.BookingStartTime));

            var days = EnumerateDays(periodStart, periodEnd);

            var dailyBreakdown = days.Select(day =>
            {
                var dayBookings = bookingsByDay[day];
                var bookedHours = dayBookings.Sum(b => (decimal)(b.BookingEndTime - b.BookingStartTime).TotalHours);

                return new DailyOccupancy
                {
                    Date = day,
                    BookedHours = bookedHours,
                    AvailableHours = DailyAvailableHours,
                    OccupancyPercentage = DailyAvailableHours == 0
                        ? 0
                        : Math.Round(bookedHours / DailyAvailableHours * 100, 2),
                    BookingsCount = dayBookings.Count()
                };
            }).ToList();

            var totalBooked = dailyBreakdown.Sum(d => d.BookedHours);
            var totalAvailable = dailyBreakdown.Sum(d => d.AvailableHours);

            return new RoomOccupancy
            {
                RoomID = room.ID,
                RoomName = room.Name,
                DailyBreakdown = dailyBreakdown,
                TotalBookedHours = totalBooked,
                TotalAvailableHours = totalAvailable,
                OccupancyPercentage = totalAvailable == 0
                    ? 0
                    : Math.Round(totalBooked / totalAvailable * 100, 2)
            };
        }

        private static OccupancyReportSummary BuildSummary(List<RoomOccupancy> roomOccupancies)
        {
            var busiest = roomOccupancies.OrderByDescending(r => r.OccupancyPercentage).FirstOrDefault();
            var leastBusy = roomOccupancies.OrderBy(r => r.OccupancyPercentage).FirstOrDefault();

            var totalBooked = roomOccupancies.Sum(r => r.TotalBookedHours);
            var totalAvailable = roomOccupancies.Sum(r => r.TotalAvailableHours);

            return new OccupancyReportSummary
            {
                TotalRooms = roomOccupancies.Count,
                OverallOccupancyPercentage = totalAvailable == 0
                    ? 0
                    : Math.Round(totalBooked / totalAvailable * 100, 2),
                BusiestRoomName = busiest?.RoomName,
                BusiestRoomOccupancyPercentage = busiest?.OccupancyPercentage ?? 0,
                LeastBusyRoomName = leastBusy?.RoomName,
                LeastBusyRoomOccupancyPercentage = leastBusy?.OccupancyPercentage ?? 0
            };
        }

        private static IEnumerable<DateOnly> EnumerateDays(DateOnly periodStart, DateOnly periodEnd)
        {
            for (var day = periodStart; day <= periodEnd; day = day.AddDays(1))
                yield return day;
        }
    }
}
