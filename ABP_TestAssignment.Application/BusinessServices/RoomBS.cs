using ABP_TestAssignment.Application.DTOs.Rooms;
using ABP_TestAssignment.Application.IBusinessServices;
using ABP_TestAssignment.Domain.Entities.Rooms;
using ABP_TestAssignment.Infrastructure.IRepositories.Rooms;
using AutoMapper;

namespace ABP_TestAssignment.Application.BusinessServices
{
    public class RoomBS : IRoomBS
    {
        private readonly IRoomRepository _roomRep;
        private readonly IMapper _mapper;

        public RoomBS(IRoomRepository roomRep, IMapper mapper)
        {
            _roomRep = roomRep;
            _mapper = mapper;
        }

        public async Task<OpRes<long>> AddAsync(AddRoomDTO dto)
        {
            if (await _roomRep.RoomExistsAsync(dto.Name))
                return OpRes.Err<long>("Room with specified name already exists");

            var room = _mapper.Map<Room>(dto);

            await _roomRep.AddAsync(room);
            await _roomRep.SaveChangesAsync();

            return OpRes.Success(room.ID);
        }

        public async Task<OpRes<bool>> DeleteAsync(long id)
        {
            var room = await _roomRep.GetByIDAsync(id);
            if (room is null)
                return OpRes.Err<bool>("Room not found");

            _roomRep.Delete(room);
            await _roomRep.SaveChangesAsync();

            return OpRes.Success(true);
        }

        public async Task<List<RoomDTO>> GetAllAsync()
        {
            var rooms = await _roomRep.GetAllRoomsAsync();
            return _mapper.Map<List<RoomDTO>>(rooms);
        }

        public async Task<RoomDTO?> GetByIDAsync(long id)
        {
            var room = await _roomRep.GetByIDAsync(id);
            return _mapper.Map<RoomDTO?>(room);
        }

        public async Task<OpRes<RoomDTO>> UpdateAsync(UpdateRoomDTO dto)
        {
            var existingRoom = await _roomRep.GetByIDAsync(dto.ID);
            if (existingRoom is null)
                return OpRes.Err<RoomDTO>("Room not found");

            if (await _roomRep.RoomExistsAsync(dto.Name, excludeID: dto.ID))
                return OpRes.Err<RoomDTO>("Room with specified name already exists");

            _mapper.Map(dto, existingRoom);
            _roomRep.Update(existingRoom);

            if (dto.AvailableServicesIDs != null)
                await _roomRep.UpdateAvailableServices(dto.ID, dto.AvailableServicesIDs);

            await _roomRep.SaveChangesAsync();
            return OpRes.Success(_mapper.Map<RoomDTO>(existingRoom));
        }
    }
}
