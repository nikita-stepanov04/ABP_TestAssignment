using ABP_TestAssignment.Domain.Entities.Rooms;

namespace ABP_TestAssignment.Infrastructure.IRepositories.Rooms
{
    public interface IRoomRepository : IRepositoryBase<Room>
    {
        Task UpdateAvailableServices(long roomId, List<long> serviceIDs);
        Task<bool> RoomExistsAsync(string name, long? excludeID = null);
        Task<Room?> GetByIDForUpdateAsync(long roomID);
        Task<List<Room>> GetAllRoomsAsync(int? capacity = null, DateTime? startTime = null, DateTime? endTime = null);
        Task<List<Room>> GetByIdsAsync(List<long> roomIDs, CancellationToken cancellationToken = default);
    }
}
