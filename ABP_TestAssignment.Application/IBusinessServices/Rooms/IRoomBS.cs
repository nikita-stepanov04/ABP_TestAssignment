using ABP_TestAssignment.Application.DTOs.Rooms;

namespace ABP_TestAssignment.Application.IBusinessServices.Rooms
{
    public interface IRoomBS
    {
        Task<List<RoomDTO>> GetAllAsync(SearchForRoomDTO dto);
        Task<RoomDTO?> GetByIDAsync(long id);
        Task<OpRes<long>> AddAsync(AddRoomDTO dto);
        Task<OpRes<bool>> DeleteAsync(long id);
        Task<OpRes<RoomDTO>> UpdateAsync(UpdateRoomDTO room);
    }
}
