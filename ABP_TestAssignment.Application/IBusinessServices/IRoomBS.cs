using ABP_TestAssignment.Application.DTOs.Rooms;

namespace ABP_TestAssignment.Application.IBusinessServices
{
    public interface IRoomBS
    {
        Task<List<RoomDTO>> GetAllAsync();
        Task<RoomDTO?> GetByIDAsync(long id);
        Task<OpRes<long>> AddAsync(AddRoomDTO dto);
        Task<OpRes<bool>> DeleteAsync(long id);
        Task<OpRes<RoomDTO>> UpdateAsync(UpdateRoomDTO room);
    }
}
