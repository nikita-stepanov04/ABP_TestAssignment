using ABP_TestAssignment.Application;
using ABP_TestAssignment.Domain.DI;
using ABP_TestAssignment.Infrastructure.EFRepositories;
using ABP_TestAssignment.Web.Configuration;
using ABP_TestAssignment.Web.Identity;

namespace ABP_TestAssignment.Web
{
    public partial class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var config = builder.Configuration;

            builder.SetupLogger();

            services.AddControllers()
                .SetUpJsonOptions();

            services.AddEndpointsApiExplorer();
            services.SetUpSwagger();

            new List<IDependencyInjectionManager>
            {
                new EFInfrastructureDIManager(),
                new DefaultApplicationDIManager()
            }.ForEach(di => di.SetupDI(services, config));

            services.SetUpIdentity(config);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGet("/", async context => context.Response.Redirect("/swagger/index.html", false));
            app.MapControllers();

            app.Services.ApplyMigrations();

            app.Run();
        }
    }
}
