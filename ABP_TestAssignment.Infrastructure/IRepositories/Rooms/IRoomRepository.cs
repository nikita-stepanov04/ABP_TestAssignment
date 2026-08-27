using ABP_TestAssignment.Domain.Entities.Rooms;

namespace ABP_TestAssignment.Infrastructure.IRepositories.Rooms
{
    public interface IRoomRepository : IRepositoryBase<Room>
    {
        Task<List<Room>> GetAllRoomsAsync();
        Task UpdateAvailableServices(long roomId, List<long> serviceIDs);
        Task<bool> RoomExistsAsync(string name, long? excludeID = null);
    }
}
