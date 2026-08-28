using ABP_TestAssignment.Application.DTOs.Rooms;
using ABP_TestAssignment.Application.IBusinessServices.Rooms;
using ABP_TestAssignment.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABP_TestAssignment.Web.Controllers
{
    [Route("api/rooms")]
    public class RoomController : ABP_TestAssignmentControllerBase
    {
        private readonly IRoomBS _roomBS;

        public RoomController(IRoomBS roomBS)
        {
            _roomBS = roomBS;
        }

        [HttpGet("get/{id}")]
        [ProducesResponseType<RoomDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType<MessageResponse>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var room = await _roomBS.GetByIDAsync(id);
            if (room is null)
                return NotFound(new MessageResponse("Room not found"));

            return Ok(room);
        }

        [HttpGet("all")]
        [ProducesResponseType<RoomDTO>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roomBS.GetAllAsync());
        }

        [HttpPost("add")]
        [Authorize(Policy = Policies.AuthorizedAdmins)]
        [ProducesResponseType<IDResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<MessageResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AddRoomDTO dto)
        {
            var result = await _roomBS.AddAsync(dto);

            if (result.HasError)
                return BadRequest(new MessageResponse(result.ErrorMessage));

            return Ok(new IDResponse(result.Result));
        }

        [HttpPut("update")]
        [Authorize(Policy = Policies.AuthorizedAdmins)]
        [ProducesResponseType<RoomDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType<MessageResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateRoomDTO dto)
        {
            var result = await _roomBS.UpdateAsync(dto);

            if (result.HasError)
                return BadRequest(new MessageResponse(result.ErrorMessage));

            return Ok(result.Result);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = Policies.AuthorizedAdmins)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<MessageResponse>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _roomBS.DeleteAsync(id);

            if (result.HasError)
                return NotFound(new MessageResponse(result.ErrorMessage));

            return Ok();
        }
    }
}
