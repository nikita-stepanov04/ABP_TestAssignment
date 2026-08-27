using ABP_TestAssignment.Application.Cryptography;
using ABP_TestAssignment.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ABP_TestAssignment.Web.Controllers
{
    [ValidateModel]
    [Produces("application/json")]
    public class ABP_TestAssignmentControllerBase : ControllerBase
    {
        public string AccessToken => GetAccessTokenFromRequest();

        private string GetAccessTokenFromRequest()
        {
            string header = HttpContext.Request.Headers["Authorization"]!;
            return header.Substring("Bearer ".Length).Trim();
        }

        public Guid CompanyID => Guid.Parse(User.FindFirst(JwtClaimType.CompanyID)!.Value);
        public string CompanyEmail => User.FindFirst(JwtClaimType.Email)!.Value;
    }
}
