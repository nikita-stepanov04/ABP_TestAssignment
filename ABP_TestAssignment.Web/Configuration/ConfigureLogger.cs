using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Settings.Configuration;

namespace ABP_TestAssignment.Web.Configuration
{
    public static class ConfigureLogger
    {
        public static void SetupLogger(this WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(
                    builder.Configuration,
                    new ConfigurationReaderOptions
                    {
                        SectionName = "Serilog"
                    })
                .Enrich.FromLogContext()
                .Enrich.With<ShortSourceContextEnricher>()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {ClassName}: {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            Log.Logger = logger;
            builder.Host.UseSerilog(logger, dispose: true);
        }
    }

    public class ShortSourceContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Properties.TryGetValue("SourceContext", out var value))
            {
                var fullName = value.ToString().Trim('"');
                var shortName = fullName.Split('.').Last();
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ClassName", shortName));
            }
        }
    }
}
