using ABP_TestAssignment.Application.DTOs.Rooms;
using ABP_TestAssignment.Domain.Entities.Rooms;
using AutoMapper;

namespace ABP_TestAssignment.Application.AutoMapper.Profiles
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<AddRoomDTO, Room>();
            CreateMap<UpdateRoomDTO, Room>();
            CreateMap<Room, RoomDTO>();
        }
    }
}
