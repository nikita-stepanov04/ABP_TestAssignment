using ABP_TestAssignment.Domain.Entities.Companies;
using ABP_TestAssignment.Infrastructure.EFRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_TestAssignment.Infrastructure.EFRepositories.Companies
{
    public class EFCompanyOnModelCreating : EFOnModelCreatingBase<Company>
    {
        protected override void OnModelCreating(EntityTypeBuilder<Company> model)
        {
            model.HasIndex(e => e.Email)
                .IsUnique();

            model.Property(e => e.Email)
                .HasColumnType("varchar(50)");

            model.Property(e => e.Name)
                .HasColumnType("varchar(50)");
        }
    }
}
