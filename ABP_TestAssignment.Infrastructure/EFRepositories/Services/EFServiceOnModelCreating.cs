using ABP_TestAssignment.Domain.Entities.Services;
using ABP_TestAssignment.Infrastructure.EFRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_TestAssignment.Infrastructure.EFRepositories.Services
{
    public class EFServiceOnModelCreating : EFOnModelCreatingBase<Service>
    {
        protected override void OnModelCreating(EntityTypeBuilder<Service> model)
        {
            model.Property(e => e.Name)
                .HasColumnType("varchar(50)");

            model.HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
