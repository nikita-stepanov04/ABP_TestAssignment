using ABP_TestAssignment.Domain.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ABP_TestAssignment.Application.BusinessServices.Bookings.PricingRules;

namespace ABP_TestAssignment.Application.BusinessServices.Bookings
{
    public class BookingPriceRuleDIManager : IDependencyInjectionManager
    {
        public IServiceCollection SetupDI(IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IPriceRule, StandardPriceRule>();
            services.AddSingleton<IPriceRule, MorningDiscountRule>();
            services.AddSingleton<IPriceRule, EveningDiscountRule>();
            services.AddSingleton<IPriceRule, PeakSurchargeRule>();

            services.AddSingleton<BookingPriceCalculator>();

            return services;
        }
    }
}
