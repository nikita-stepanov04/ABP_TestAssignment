using ABP_TestAssignment.Application.DTOs.Reports.Revenue;

namespace ABP_TestAssignment.Application.IBusinessServices.Reports
{
    public interface IRevenueReportBS
    {
        Task<RevenueReportResponse> GenerateAsync(
            RevenueReportRequest request, CancellationToken cancellationToken = default);
    }
}
