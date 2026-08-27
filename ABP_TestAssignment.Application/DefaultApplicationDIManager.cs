using ABP_TestAssignment.Application.Jobs;
using ABP_TestAssignment.Domain.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ABP_TestAssignment.Application
{
    public class DefaultApplicationDIManager : IDependencyInjectionManager
    {
        public IServiceCollection SetupDI(IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(cfg => { }, typeof(DefaultApplicationDIManager).Assembly);
            services.SetUpJobs();

            services.AddOptions<JwtSettings>()
                .BindConfiguration("Jwt")
                .ValidateDataAnnotations()
                .Validate(
                    validation: s => s.AccessTokenKey != s.RefreshTokenKey,
                    failureMessage: "Access token can not be equal to refresh token"
                ).ValidateOnStart();

            services.AddOptions<AdminSettings>()
                .BindConfiguration("Admins")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
