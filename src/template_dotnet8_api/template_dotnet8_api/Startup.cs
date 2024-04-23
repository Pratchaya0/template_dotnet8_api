using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using System;
using template_dotnet8_api.Configurations;
using template_dotnet8_api.Data;
using template_dotnet8_api.Middlewares;
using template_dotnet8_api.Startups;

namespace template_dotnet8_api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private ProjectSetting ProjectConfiguration { get; }
        private QuartzSetting QuartzConfiguration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            if (configuration.GetSection("Project") is null)
                throw new ArgumentNullException("Project is not set in appsetting.json");

            ProjectConfiguration = configuration.GetSection("Project")?.Get<ProjectSetting>();
            QuartzConfiguration = configuration.GetSection("Quartz")?.Get<QuartzSetting>();
        }

        // Method นี้จะถูกเรียกโดย runtime จะใช้ method นี้ในการเพิ่ม services เข้า container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ErrorFilter));
                options.Filters.Add(new ValidateModelFilter());
            })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                });

            services.AddHttpContextAccessor();
            services.AddResponseCaching();

            // CORS *
            services.AddCors(options => options.AddPolicy("MyPolicy", builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
            );

            // HealthChecks
            //services.AddHealthChecks()
            //    .AddDbContextCheck<AppContext>(tags: new[] { "ready" })
            //    .ForwardToPrometheus();

            // AutoMapper *
            services.AddAutoMapper(typeof(Startup));

            // DBContext *
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            // Dependency Injection *
            services.ConfigDependency(Configuration);

            // Quart (Cron Job) *
            services.AddQuartz(QuartzConfiguration);

            // Swagger (API Document) *
            services.AddSwaggerWithOutAuth(ProjectConfiguration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configere(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseSwaggerWithOutAuth(ProjectConfiguration);

            app.UseStaticFiles();

            app.UseRouting();

            //app.UseHttpMetrics();

            app.UseResponseCaching();


            // CORS
            app.UseCors("MyPolicy");

            // HealthChecks
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
            //    {
            //        ResponseWriter = HealthCheckResponseWriter.WriteResponseReadiness,
            //        Predicate = (check) => !check.Tags.Contains("ready")
            //    });

            //    endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
            //    {
            //        ResponseWriter = HealthCheckResponseWriter.WriteResponseLiveness,
            //        Predicate = (check) => !check.Tags.Contains("ready")
            //    });

            //    endpoints.MapMetrics();
            //});

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
