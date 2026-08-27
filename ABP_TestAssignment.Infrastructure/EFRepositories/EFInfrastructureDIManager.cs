using ABP_TestAssignment.Domain.DI;
using ABP_TestAssignment.Infrastructure.EFRepositories.Companies;
using ABP_TestAssignment.Infrastructure.EFRepositories.Services;
using ABP_TestAssignment.Infrastructure.EFRepository;
using ABP_TestAssignment.Infrastructure.IRepositories.Companies;
using ABP_TestAssignment.Infrastructure.IRepositories.Services;
using ABP_TestAssignment.Infrastructure.IRepositories.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ABP_TestAssignment.Infrastructure.EFRepositories
{
    public class EFInfrastructureDIManager : IDependencyInjectionManager
    {
        public IServiceCollection SetupDI(IServiceCollection services, IConfiguration config)
        {
            string? dbConnection = config.GetConnectionString("DbConnection");

            if (dbConnection == null) throw new ArgumentNullException("DbConnection is not defined");

            services.AddDbContext<EFDataContext>(opts =>
            {
                opts.UseLazyLoadingProxies();
                opts.UseNpgsql(dbConnection, dbOpts =>
                    dbOpts.MigrationsAssembly("ABP_TestAssignment.Infrastructure"));

                #if DEBUG
                    opts.EnableSensitiveDataLogging();
                #endif
            });

            services.AddScoped<ICompanyRepository, EFCompanyRepository>();
            services.AddScoped<IServiceRepository, EFServiceRepository>();
            services.AddScoped<IInvalidatedTokenRepository, EFInvalidatedTokenRepository>();

            return services;
        }
    }

    public static class StartUpDb
    {
        public static void ApplyMigrations(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<EFDataContext>();
                db.Database.Migrate();
            }
        }
    }
}
