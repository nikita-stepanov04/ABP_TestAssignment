using ABP_TestAssignment.Application.DTOs.Services;
using ABP_TestAssignment.Application.IBusinessServices;
using ABP_TestAssignment.Domain.Entities.Services;
using ABP_TestAssignment.Infrastructure.IRepositories.Services;
using AutoMapper;

namespace ABP_TestAssignment.Application.BusinessServices
{
    public class ServiceBS : IServiceBS
    {
        private readonly IServiceRepository _serviceRep;
        private readonly IMapper _mapper;

        public ServiceBS(
            IServiceRepository serviceRep,
            IMapper mapper)
        {
            _serviceRep = serviceRep;
            _mapper = mapper;
        }

        public async Task<OpRes<long>> AddServiceAsync(AddServiceDTO dto)
        {
            if (await _serviceRep.ServiceExistsAsync(dto.Name))
                return OpRes.Err<long>("Service with this name already exists");

            var service = _mapper.Map<Service>(dto);

            await _serviceRep.AddAsync(service);
            await _serviceRep.SaveChangesAsync();

            return OpRes.Success(service.ID);
        }

        public async Task<List<ServiceDTO>> GetAllAsync()
        {
            var services = await _serviceRep.GetAllAsync();
            return _mapper.Map<List<ServiceDTO>>(services);
        }
    }
}
