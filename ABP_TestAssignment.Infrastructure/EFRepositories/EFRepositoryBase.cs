using ABP_TestAssignment.Domain.Entities;
using ABP_TestAssignment.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ABP_TestAssignment.Infrastructure.EFRepositories
{
    public class EFRepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : EntityBase
    {
        protected EFDataContext DbContext { get; }

        public DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        public EFRepositoryBase(EFDataContext context)
        {
            DbContext = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity?> GetByIDAsync(Guid id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
