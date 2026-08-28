using ABP_TestAssignment.Application.BusinessServices.Bookings;
using ABP_TestAssignment.Application.BusinessServices.Companies;
using ABP_TestAssignment.Application.BusinessServices.Rooms;
using ABP_TestAssignment.Application.BusinessServices.Services;
using ABP_TestAssignment.Application.BusinessServices.Tokens;
using ABP_TestAssignment.Application.IBusinessServices.Bookings;
using ABP_TestAssignment.Application.IBusinessServices.Companies;
using ABP_TestAssignment.Application.IBusinessServices.Rooms;
using ABP_TestAssignment.Application.IBusinessServices.Services;
using ABP_TestAssignment.Application.IBusinessServices.Tokens;
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

            services.AddScoped<IRoomBS, RoomBS>();
            services.AddScoped<ITokenBS, TokenBS>();
            services.AddScoped<IBookingBS, BookingBS>();
            services.AddScoped<ICompanyBS, CompanyBS>();
            services.AddScoped<IServiceBS, ServiceBS>();

            var bookingPricingDIManager = new BookingPriceRuleDIManager();
            bookingPricingDIManager.SetupDI(services, config);

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
