using ABP_TestAssignment.Application.DTOs.Rooms;
using ABP_TestAssignment.Domain.Entities.Rooms;
using ABP_TestAssignment.Domain.Entities.Services;
using AutoMapper;

namespace ABP_TestAssignment.Application.AutoMapper.Profiles
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<AddRoomDTO, Room>()
                .ForMember(dest => dest.AvailableServices, opt => opt.MapFrom(src =>
                    src.AvailableServicesIDs != null
                        ? src.AvailableServicesIDs.Select(id => new Service { ID = id }).ToList()
                        : new List<Service>()));

            CreateMap<UpdateRoomDTO, Room>();
            CreateMap<Room, RoomDTO>();
        }
    }
}
