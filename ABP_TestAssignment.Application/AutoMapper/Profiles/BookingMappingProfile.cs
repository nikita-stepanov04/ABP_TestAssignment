using ABP_TestAssignment.Application.DTOs.Bookings;
using ABP_TestAssignment.Domain.Entities.Bookings;
using AutoMapper;

namespace ABP_TestAssignment.Application.AutoMapper.Profiles
{
    public class BookingMappingProfile : Profile
    {
        public BookingMappingProfile()
        {
            CreateMap<BookRoomDTO, Booking>()
                .ForMember(
                    dest => dest.BookingStartTime,
                    opt => opt.MapFrom(src => src.StartTime)
                )
                .ForMember(
                    dest => dest.BookingEndTime,
                    opt => opt.MapFrom(src => src.EndTime)
                );
        }
    }
}
