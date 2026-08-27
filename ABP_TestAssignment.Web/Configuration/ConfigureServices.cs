using System.Text.Json.Serialization;

namespace ABP_TestAssignment.Web
{
    public static class ConfigureServices
    {
        public static IMvcBuilder SetUpJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            return builder;
        }
    }
}
