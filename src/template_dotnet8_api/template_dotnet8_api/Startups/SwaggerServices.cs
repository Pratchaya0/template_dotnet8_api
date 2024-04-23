using Microsoft.OpenApi.Models;
using template_dotnet8_api.Configurations;

namespace template_dotnet8_api.Startups
{
    public static class SwaggerServices
    {
        public static IServiceCollection AddSwaggerWithOutAuth(this IServiceCollection services, ProjectSetting projectSetting)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = projectSetting.Version,
                        Title = projectSetting.Title,
                        Description = projectSetting.Description,
                    });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerWithOutAuth(this IApplicationBuilder app, ProjectSetting projectSetting)
        {
            app.UseSwagger(config =>
            {
                config.PreSerializeFilters.Add((swagger, httpRequest) =>
                {
                    swagger.Servers.Clear();
                });
            });

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", projectSetting.Title);
            });

            return app;
        }
    }
}
