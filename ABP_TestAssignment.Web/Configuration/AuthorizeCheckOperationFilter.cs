using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ABP_TestAssignment.Web.Configuration
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = 
                context.MethodInfo.DeclaringType!
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any() ||
                context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any();

            var hasAllowAnonymous = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AllowAnonymousAttribute>()
                .Any();

            if (hasAuthorize && !hasAllowAnonymous)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
                    }
                };
            }
            else
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
            }
        }
    }
}
