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

        public Task<List<Room>> GetAllRoomsAsync(
            int? capacity = null,
            DateTime? startTime = null,
            DateTime? endTime = null)
        {
            var query = DbSet
                .Include(r => r.AvailableServices)
                .AsQueryable();

            if (capacity.HasValue)
                query = query.Where(r => r.Capacity >= capacity.Value);

            if (startTime.HasValue && endTime.HasValue)
                query = query.Where(r =>
                    !r.Bookings.Any(b =>
                        (startTime.Value < b.BookingEndTime) &&
                        (endTime.Value > b.BookingStartTime)));

            return query.AsNoTracking()
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

        public Task<Room?> GetByIDForUpdateAsync(long roomID)
        {
            return DbSet
                .FromSqlInterpolated($@"SELECT * FROM ""Room"" WHERE ""ID"" = {roomID} FOR UPDATE")
                .Include(r => r.AvailableServices)
                .SingleOrDefaultAsync();
        }

        public Task<List<Room>> GetByIdsAsync(
            List<long> roomIDs,
            CancellationToken cancellationToken = default)
        {
            return DbSet
                .Where(r => roomIDs.Contains(r.ID))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
