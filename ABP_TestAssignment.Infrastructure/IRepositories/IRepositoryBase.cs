using ABP_TestAssignment.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ABP_TestAssignment.Infrastructure.IRepositories
{
    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        Task<TEntity?> GetByIDAsync(long id);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
