using Microsoft.OpenApi;

namespace ABP_TestAssignment.Web
{
    public static class SwaggerConfig
    {
        public static IServiceCollection SetUpSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ABP_TestAssignment", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insert jwt token: Bearer {your token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement()
                {
                    [new OpenApiSecuritySchemeReference("Bearer", doc)] = []
                });
            });
            return services;
        }
    }
}
