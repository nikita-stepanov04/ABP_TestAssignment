using ABP_TestAssignment.Domain.Entities.Services;

namespace ABP_TestAssignment.Infrastructure.IRepositories.Services
{
    public interface IServiceRepository : IRepositoryBase<Service>
    {
        Task<List<Service>> GetAllAsync(List<long>? ids = null);
        Task<bool> ServiceExistsAsync(string name);
    }
}
