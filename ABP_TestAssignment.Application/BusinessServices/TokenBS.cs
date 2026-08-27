using ABP_TestAssignment.Application.Cryptography;
using ABP_TestAssignment.Application.DTOs.Companies;
using ABP_TestAssignment.Application.IBusinessServices;
using ABP_TestAssignment.Domain.Entities.Tokens;
using ABP_TestAssignment.Infrastructure.IRepositories.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ABP_TestAssignment.Application.BusinessServices
{
    public class TokenBS : ITokenBS
    {
        private readonly IInvalidatedTokenRepository _tokenRep;
        private readonly JwtSettings _jwtSettings;

        private JwtSecurityTokenHandler Decoder = new JwtSecurityTokenHandler();

        const int EXPIRED_TOKEN_REMOVAL_OFFSET = 30; // Minutes

        public TokenBS(
            IInvalidatedTokenRepository tokenRep,
            IOptions<JwtSettings> jwtSettings)
        {
            _tokenRep = tokenRep;
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateAccessToken(CompanyDTO company)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtClaimType.Email, company.Email),
                new Claim(JwtClaimType.CompanyID, company.ID.ToString()),
                new Claim(ClaimTypes.Role, company.Role.ToString()),
                new Claim(JwtClaimType.TokenID, NewRandomGUID())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.AccessTokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var claim = new Claim(JwtClaimType.TokenID, NewRandomGUID());

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: [claim],
                expires: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IEnumerable<Claim>? ValidateAccessTokenForRefresh(string token) => ValidateToken(token, true, false);
        public IEnumerable<Claim>? ValidateRefreshTokenForRefresh(string token) => ValidateToken(token, false, true);

        private IEnumerable<Claim>? ValidateToken(string token, bool isAccess, bool validateLifetime)
        {
            SecurityToken? validatedToken = null;
            try
            {
                Decoder.ValidateToken(
                    token: token,
                    validationParameters: Jwt.GetTokenValidationParameters(
                        isAccess ? _jwtSettings.AccessTokenKey : _jwtSettings.RefreshTokenKey,
                        validateLifetime
                    ),
                    validatedToken: out validatedToken
                );
            }
            catch { }

            if (validatedToken == null)
                return null;

            return (validatedToken as JwtSecurityToken)!.Claims;
        }


        public async Task<bool> RevokeToken(string token, bool isAccess)
        {
            var jwtToken = Decoder.ReadJwtToken(token);

            if (await IsTokenRevokedAsync(jwtToken))
                return false;

            var tokenID = GetTokenID(jwtToken);
            var expDate = jwtToken.ValidTo;

            var invalidatedToken = new InvalidatedToken
            {
                TokenID = tokenID,
                DateExpiration = expDate
            };

            await _tokenRep.AddAsync(invalidatedToken);
            await _tokenRep.SaveChangesAsync();
            return true;
        }

        public async Task<InvalidatedToken?> GetByIDAsync(long id)
        {
            return await _tokenRep.GetByIDAsync(id);
        }

        public async Task<bool> IsTokenRevokedAsync(string token)
        {
            var jwtToken = Decoder.ReadJwtToken(token);
            return await IsTokenRevokedAsync(jwtToken);
        }

        public async Task<bool> IsTokenRevokedAsync(JwtSecurityToken jwtToken)
        {
            var tokenID = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtClaimType.TokenID)!.Value;
            return await _tokenRep.IsTokenPresentAsync(tokenID);
        }

        private string GetTokenID(JwtSecurityToken jwtToken) => jwtToken.Claims.FirstOrDefault(c => c.Type == JwtClaimType.TokenID)!.Value;
        private string NewRandomGUID() => Guid.NewGuid().ToString().Replace("-", string.Empty);

        public Task RemoveExpiredInvalidatedTokens()
        {
            return _tokenRep.RemoveInvalidatedTokensExpiredBefore(
                DateTime.UtcNow.AddMinutes(-EXPIRED_TOKEN_REMOVAL_OFFSET));
        }
    }
}
