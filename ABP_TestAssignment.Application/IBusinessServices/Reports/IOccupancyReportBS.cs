using ABP_TestAssignment.Application.DTOs.Reports.Occupancy;

namespace ABP_TestAssignment.Application.IBusinessServices.Reports
{
    public interface IOccupancyReportBS
    {
        Task<OccupancyReportResponse> GenerateAsync(
            OccupancyReportRequest request, CancellationToken cancellationToken = default);
    }
}
