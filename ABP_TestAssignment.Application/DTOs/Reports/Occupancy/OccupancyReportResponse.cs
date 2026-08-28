namespace ABP_TestAssignment.Application.DTOs.Reports.Occupancy
{
    public class OccupancyReportResponse
    {
        public DateOnly PeriodStart { get; init; }
        public DateOnly PeriodEnd { get; init; }
        public DateTime GeneratedAt { get; init; }

        public List<RoomOccupancy> Rooms { get; init; } = new();
        public OccupancyReportSummary Summary { get; init; } = null!;
    }

    public class RoomOccupancy
    {
        public long RoomID { get; init; }
        public string RoomName { get; init; } = string.Empty;

        public List<DailyOccupancy> DailyBreakdown { get; init; } = new();

        public decimal TotalBookedHours { get; init; }
        public decimal TotalAvailableHours { get; init; }
        public decimal OccupancyPercentage { get; init; }
    }

    public class DailyOccupancy
    {
        public DateOnly Date { get; init; }
        public decimal BookedHours { get; init; }
        public decimal AvailableHours { get; init; }
        public decimal OccupancyPercentage { get; init; }
        public int BookingsCount { get; init; }
    }

    public class OccupancyReportSummary
    {
        public int TotalRooms { get; init; }
        public decimal OverallOccupancyPercentage { get; init; }

        public string? BusiestRoomName { get; init; }
        public decimal BusiestRoomOccupancyPercentage { get; init; }

        public string? LeastBusyRoomName { get; init; }
        public decimal LeastBusyRoomOccupancyPercentage { get; init; }
    }
}
