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

        public virtual async Task AddAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task<TEntity?> GetByIDAsync(long id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
    }
}
