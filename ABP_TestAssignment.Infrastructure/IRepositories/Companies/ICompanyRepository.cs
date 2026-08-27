using ABP_TestAssignment.Domain.Entities.Companies;

namespace ABP_TestAssignment.Infrastructure.IRepositories.Companies
{
    public interface ICompanyRepository : IRepositoryBase<Company>
    {
        public Task<Company?> GetByEmailAsync(string email);
    }
}
