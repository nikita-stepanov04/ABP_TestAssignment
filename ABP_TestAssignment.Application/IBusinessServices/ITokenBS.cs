using ABP_TestAssignment.Application.DTOs.Companies;
using ABP_TestAssignment.Domain.Entities.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ABP_TestAssignment.Application.IBusinessServices
{
    public interface ITokenBS
    {
        string GenerateAccessToken(CompanyDTO company);
        string GenerateRefreshToken();
        IEnumerable<Claim>? ValidateAccessTokenForRefresh(string token);
        IEnumerable<Claim>? ValidateRefreshTokenForRefresh(string token);
        Task<bool> RevokeToken(string token, bool isAccess);
        Task<InvalidatedToken?> GetByIDAsync(long id);
        Task<bool> IsTokenRevokedAsync(string token);
        Task<bool> IsTokenRevokedAsync(JwtSecurityToken jwtToken);
        Task RemoveExpiredInvalidatedTokens();
    }
}
