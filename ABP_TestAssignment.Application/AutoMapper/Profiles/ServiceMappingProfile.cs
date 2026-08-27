using ABP_TestAssignment.Application.DTOs.Services;
using ABP_TestAssignment.Domain.Entities.Services;
using AutoMapper;

namespace ABP_TestAssignment.Application.AutoMapper.Profiles
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<Service, ServiceDTO>();
            CreateMap<AddServiceDTO, Service>();
        }
    }
}
