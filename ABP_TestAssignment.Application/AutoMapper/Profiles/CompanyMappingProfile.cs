using ABP_TestAssignment.Application.DTOs.Companies;
using ABP_TestAssignment.Domain.Entities.Companies;
using AutoMapper;

namespace ABP_TestAssignment.Application.AutoMapper.Profiles
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            CreateMap<Company, CompanyDTO>();
            CreateMap<CompanyRegistrationDTO, Company>();
        }
    }
}
