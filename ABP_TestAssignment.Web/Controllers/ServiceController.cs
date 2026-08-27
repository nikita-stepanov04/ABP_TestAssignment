using ABP_TestAssignment.Application.DTOs.Services;
using ABP_TestAssignment.Application.IBusinessServices;
using ABP_TestAssignment.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABP_TestAssignment.Web.Controllers
{
    [Route("api/services")]
    public class ServiceController : ABP_TestAssignmentControllerBase
    {
        private readonly IServiceBS _serviceBS;

        public ServiceController(IServiceBS serviceBS)
        {
            _serviceBS = serviceBS;
        }

        [HttpGet("all")]
        [ProducesResponseType<List<ServiceDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var services = await _serviceBS.GetAllAsync();
            return Ok(services);
        }

        [HttpPost("add")]
        [Authorize(Policy = Policies.AuthorizedAdmins)]
        [ProducesResponseType<IDResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<MessageResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddService([FromBody] AddServiceDTO dto)
        {
            var result = await _serviceBS.AddServiceAsync(dto);

            if (result.HasError)
                return BadRequest(new MessageResponse(result.ErrorMessage));

            return Ok(new IDResponse(result.Result));
        }

    }
}
