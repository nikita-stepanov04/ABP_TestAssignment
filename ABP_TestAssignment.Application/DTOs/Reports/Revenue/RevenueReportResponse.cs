namespace ABP_TestAssignment.Application.DTOs.Reports.Revenue
{
    public class RevenueReportResponse
    {
        public DateOnly PeriodStart { get; init; }
        public DateOnly PeriodEnd { get; init; }
        public DateTime GeneratedAt { get; init; }
        public List<DailyRevenue> DailyBreakdown { get; init; } = new();

        public decimal TotalRoomRevenue { get; init; }
        public decimal TotalServiceRevenue { get; init; }
        public decimal TotalRevenue { get; init; }
    }

    public class DailyRevenue
    {
        public DateOnly Date { get; init; }
        public decimal RoomRevenue { get; init; }
        public decimal ServiceRevenue { get; init; }
        public decimal TotalRevenue { get; init; }
        public int BookingsCount { get; init; }
    }
}
