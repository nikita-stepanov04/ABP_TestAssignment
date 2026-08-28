using ABP_TestAssignment.Application.DTOs.Reports.Occupancy;
using ABP_TestAssignment.Application.IBusinessServices.Reports;
using ABP_TestAssignment.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ABP_TestAssignment.Application.DTOs.Reports.Revenue;

namespace ABP_TestAssignment.Web.Controllers
{
    [Route("api/reports")]
    [Authorize(Policy = Policies.AuthorizedAdmins)]
    public class ReportsController : ABP_TestAssignmentControllerBase
    {
        private readonly IOccupancyReportBS _occupancyReportBS;
        private readonly IRevenueReportBS _revenueReportBS;

        public ReportsController(
            IOccupancyReportBS occupancyReportBS, 
            IRevenueReportBS revenueReportBS)
        {
            _occupancyReportBS = occupancyReportBS;
            _revenueReportBS = revenueReportBS;
        }

        [HttpPost("occupancy")]
        [ProducesResponseType<OccupancyReportResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> OccupancyReport(
            [FromBody] OccupancyReportRequest request, CancellationToken token)
        {
            return Ok(await _occupancyReportBS.GenerateAsync(request, token));
        }

        [HttpPost("revenue")]
        [ProducesResponseType<RevenueReportResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> RevenueReport(
            [FromBody] RevenueReportRequest request, CancellationToken token)
        {
            return Ok(await _revenueReportBS.GenerateAsync(request, token));
        }
    }
}
