using ABP_TestAssignment.Domain.Entities.Rooms;
using ABP_TestAssignment.Domain.Entities.Services;
using ABP_TestAssignment.Infrastructure.IRepositories.Rooms;
using Microsoft.EntityFrameworkCore;

namespace ABP_TestAssignment.Infrastructure.EFRepositories.Rooms
{
    public class EFRoomRepository : EFRepositoryBase<Room>, IRoomRepository
    {
        public EFRoomRepository(EFDataContext context) 
            : base(context) { }

        public Task<List<Room>> GetAllRoomsAsync()
        {
            return DbSet
                .Include(r => r.AvailableServices)
                .ToListAsync();
        }

        public override Task AddAsync(Room room)
        {
            room.AvailableServices.ForEach(s => DbContext.Attach(s));
            return base.AddAsync(room);
        }

        public override Task<Room?> GetByIDAsync(long id)
        {
            return DbSet.Include(r => r.AvailableServices).FirstOrDefaultAsync(r => r.ID == id);
        }

        public override void Update(Room room)
        {
            var existingRoom = DbSet.First(r => r.ID == room.ID);
            DbContext.Entry(existingRoom).CurrentValues.SetValues(room);
            base.Update(room);
        }

        public async Task UpdateAvailableServices(long roomId, List<long> serviceIDs)
        {
            var existingRoom = await DbSet
                .Include(r => r.AvailableServices)
                .FirstAsync(r => r.ID == roomId);

            var currentIDs = existingRoom.AvailableServices.Select(s => s.ID).ToHashSet();
            var newIDs = serviceIDs.ToHashSet();

            var toRemove = existingRoom.AvailableServices
                .Where(s => !newIDs.Contains(s.ID))
                .ToList();

            foreach (var s in toRemove)
                existingRoom.AvailableServices.Remove(s);

            var idsToAdd = newIDs.Except(currentIDs).ToList();
            if (idsToAdd.Count > 0)
            {
                var servicesToAdd = DbContext.Set<Service>()
                    .Where(s => idsToAdd.Contains(s.ID))
                    .ToList();

                foreach (var service in servicesToAdd)
                    existingRoom.AvailableServices.Add(service);
            }
        }

        public Task<bool> RoomExistsAsync(string name, long? excludeID = null)
        {
            var query = DbSet.AsQueryable();

            if (excludeID != null) 
                query = query.Where(r => r.ID != excludeID);

            return query.AnyAsync(r => EF.Functions.ILike(r.Name, name));
        }
    }
}
