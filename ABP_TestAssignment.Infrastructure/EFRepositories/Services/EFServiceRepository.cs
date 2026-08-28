using ABP_TestAssignment.Domain.Entities.Services;
using ABP_TestAssignment.Infrastructure.IRepositories.Services;
using Microsoft.EntityFrameworkCore;

namespace ABP_TestAssignment.Infrastructure.EFRepositories.Services
{
    public class EFServiceRepository : EFRepositoryBase<Service>, IServiceRepository
    {
        public EFServiceRepository(EFDataContext context) 
            : base(context) { }

        public Task<List<Service>> GetAllAsync(List<long>? ids = null)
        {
            var query = DbSet.AsQueryable();

            if (ids != null)
                query = query.Where(s => ids.Contains(s.ID));

            return query.AsNoTracking()
                .ToListAsync();
        }

        public Task<bool> ServiceExistsAsync(string name)
        {
            return DbSet.AnyAsync(s => EF.Functions.ILike(s.Name, name));
        }
    }
}
