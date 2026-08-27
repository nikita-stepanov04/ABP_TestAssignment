using ABP_TestAssignment.Application.Cryptography;
using ABP_TestAssignment.Application.DTOs.Companies;
using ABP_TestAssignment.Application.IBusinessServices;
using ABP_TestAssignment.Web.Identity;
using ABP_TestAssignment.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABP_TestAssignment.Web.Controllers
{
    [Route("api/company")]
    public class CompanyController : ABP_TestAssignmentControllerBase
    {
        private readonly ICompanyBS _companyBS;
        private readonly ITokenBS _tokenBS;

        public CompanyController(
            ICompanyBS companyBS,
            ITokenBS tokenBS)
        {
            _companyBS = companyBS;
            _tokenBS = tokenBS;
        }

        [HttpGet("get/{email}")]
        [ProducesResponseType<CompanyDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            var company = await _companyBS.GetByEmailAsync(email);
            return company != null
                ? Ok(company)
                : NotFound();
        }

        [HttpGet("check-email/{email}")]
        [ProducesResponseType<BoolResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckLogin([FromRoute] string email)
        {
            var result = await _companyBS.IsEmailNotTakenAsync(email);
            return Ok(new BoolResponse(result));
        }

        [HttpPost("registration")]
        [ProducesResponseType<IDResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<MessageResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registration([FromBody] CompanyRegistrationDTO dto)
        {
            var result = await _companyBS.RegisterCompanyAsync(dto);

            if (result.HasError)
                return BadRequest(new MessageResponse(result.ErrorMessage));

            return Ok(new IDResponse(result.Result));
        }

        [HttpPost("login")]
        [ProducesResponseType<TokensResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<MessageResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] CompanyLogInDTO dto)
        {
            var user = await _companyBS.CheckCredentialsForLoginAsync(dto);

            if (user == null)
                return BadRequest(new MessageResponse($"Incorrect login or password"));

            var accessToken = _tokenBS.GenerateAccessToken(user);
            var refreshToken = _tokenBS.GenerateRefreshToken();

            return Ok(new TokensResponse(accessToken, refreshToken));
        }

        [HttpPost("logout")]
        [Authorize(Policy = Policies.AuthorizedAny)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest req)
        {
            await _tokenBS.RevokeToken(AccessToken, isAccess: true);
            await _tokenBS.RevokeToken(req.RefreshToken, isAccess: false);

            return Ok();
        }

        [HttpPost("refresh")]
        [Authorize(Policy = Policies.AuthorizedAny)]
        [ProducesResponseType<RefreshResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<MessageResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest req)
        {
            var accRes = _tokenBS.ValidateAccessTokenForRefresh(AccessToken);
            var refRes = _tokenBS.ValidateRefreshTokenForRefresh(req.RefreshToken);

            if (accRes != null && refRes != null)
            {
                var email = accRes.FirstOrDefault(c => c.Type == JwtClaimType.Email)!.Value;
                var company = await _companyBS.GetByEmailAsync(email);

                var newAccToken = _tokenBS.GenerateAccessToken(company!);
                return Ok(new RefreshResponse(newAccToken));
            }
            return BadRequest(new MessageResponse("Failed to validate tokens"));
        }
    }
}
