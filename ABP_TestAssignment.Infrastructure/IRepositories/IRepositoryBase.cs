using ABP_TestAssignment.Domain.Entities;

namespace ABP_TestAssignment.Infrastructure.IRepositories
{
    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        Task<TEntity?> GetByIDAsync(Guid id);
        Task AddAsync(TEntity entity);
        Task SaveChangesAsync();
    }
}
