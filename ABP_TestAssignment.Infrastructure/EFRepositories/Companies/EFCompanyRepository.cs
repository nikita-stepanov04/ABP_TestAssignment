using ABP_TestAssignment.Domain.Entities.Companies;
using ABP_TestAssignment.Infrastructure.IRepositories.Companies;
using Microsoft.EntityFrameworkCore;

namespace ABP_TestAssignment.Infrastructure.EFRepositories.Companies
{
    internal class EFCompanyRepository : EFRepositoryBase<Company>, ICompanyRepository
    {
        public EFCompanyRepository(EFDataContext context) 
            : base(context) { }

        public Task<Company?> GetByEmailAsync(string email)
        {
            return DbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
