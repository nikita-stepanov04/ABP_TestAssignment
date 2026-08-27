using ABP_TestAssignment.Application.DTOs.Services;

namespace ABP_TestAssignment.Application.IBusinessServices
{
    public interface IServiceBS
    {
        Task<OpRes<long>> AddServiceAsync(AddServiceDTO dto);
        Task<List<ServiceDTO>> GetAllAsync();
    }
}
