using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ABP_TestAssignment.Domain.DI
{
    public interface IDependencyInjectionManager
    {
        IServiceCollection SetupDI(IServiceCollection services, IConfiguration config);
    }
}
