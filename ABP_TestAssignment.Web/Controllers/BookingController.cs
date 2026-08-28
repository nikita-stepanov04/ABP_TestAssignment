using ABP_TestAssignment.Application.DTOs.Bookings;
using ABP_TestAssignment.Application.IBusinessServices.Bookings;
using ABP_TestAssignment.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABP_TestAssignment.Web.Controllers
{
    [Route("api/booking")]
    public class BookingController : ABP_TestAssignmentControllerBase
    {
        private readonly IBookingBS _bookingBS;

        public BookingController(IBookingBS bookingBS)
        {
            _bookingBS = bookingBS;
        }

        [HttpGet("all")]
        [Authorize(Policy = Policies.AuthorizedAny)]
        [ProducesResponseType<List<BookingDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookingBS.GetAllBookingsForCompanyAsync(CompanyID));
        }

        [HttpPost("add")]
        [Authorize(Policy = Policies.AuthorizedAny)]
        [ProducesResponseType<IDResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<MessageResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BookRoom([FromBody] BookRoomDTO dto)
        {
            var result = await _bookingBS.BookRoomAsync(dto, CompanyID);

            if (result.HasError)
                return BadRequest(new MessageResponse(result.ErrorMessage));

            return Ok(result.Result);
        }
    }
}
