using ABP_TestAssignment.Domain.Entities.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_TestAssignment.Infrastructure.EFRepository
{
    internal class EFOnInvalidatedTokenModelCreating : EFOnModelCreatingBase<InvalidatedToken>
    {
        protected override void OnModelCreating(EntityTypeBuilder<InvalidatedToken> model)
        {
            model.HasIndex(e => e.TokenID);
            model.Property(e => e.TokenID)
                .HasColumnType("varchar(32)");
        }
    }
}
