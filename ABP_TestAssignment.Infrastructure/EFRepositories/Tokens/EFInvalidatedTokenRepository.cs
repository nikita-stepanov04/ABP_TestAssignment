using ABP_TestAssignment.Domain.Entities.Tokens;
using ABP_TestAssignment.Infrastructure.EFRepositories;
using ABP_TestAssignment.Infrastructure.IRepositories.Tokens;
using Microsoft.EntityFrameworkCore;

namespace ABP_TestAssignment.Infrastructure.EFRepository
{
    public class EFInvalidatedTokenRepository : EFRepositoryBase<InvalidatedToken>, IInvalidatedTokenRepository
    {
        public EFInvalidatedTokenRepository(EFDataContext context)
            : base(context) { }

        public Task<bool> IsTokenPresentAsync(string tokenId)
        {
            return DbSet.AnyAsync(t => t.TokenID == tokenId);
        }

        public Task RemoveInvalidatedTokensExpiredBefore(DateTime date)
        {
            return DbSet
                .Where(t => t.DateExpiration < date)
                .ExecuteDeleteAsync();
        }
    }
}
