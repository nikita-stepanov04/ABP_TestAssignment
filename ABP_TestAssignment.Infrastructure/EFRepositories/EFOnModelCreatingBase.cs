using ABP_TestAssignment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_TestAssignment.Infrastructure.EFRepository
{
    public abstract class EFOnModelCreatingBase<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : EntityBase
    {
        public void Configure(EntityTypeBuilder<TEntity> model)
        {
            model.HasKey(e => e.ID);
            OnModelCreating(model);
        }

        protected abstract void OnModelCreating(EntityTypeBuilder<TEntity> model);
    }
}
