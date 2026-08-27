using ABP_TestAssignment.Domain.Entities.Tokens;

namespace ABP_TestAssignment.Infrastructure.IRepositories.Tokens
{
    public interface IInvalidatedTokenRepository : IRepositoryBase<InvalidatedToken>
    {
        Task<bool> IsTokenPresentAsync(string tokenId);

        Task RemoveInvalidatedTokensExpiredBefore(DateTime date);
    }
}
