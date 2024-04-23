using Quartz;
using template_dotnet8_api.Configurations;

namespace template_dotnet8_api.Startups
{
    public static class QuartServices
    {
        internal static IServiceCollection AddQuartz(this IServiceCollection services, QuartzSetting quartzSetting)
        {
            if (!quartzSetting.EnableQuartz)
            {
                return services;
            }

            services.AddQuartz(quartz =>
            {
                // Use a Scoped container to create jobs.
                //quartz.UseMicrosoftDependencyInjectionScopedJobFactory();
                quartz.ConfigQuartz(quartzSetting);
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            return services;
        }

        internal static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartzConfigurator, QuartzSetting quartzSetting) where T : IJob
        {
            // Use the name of the IJob as the appsettings.json key
            string jobName = typeof(T).Name;

            // Try and load the schedule from configuration
            var cronSchedule = quartzSetting.Jobs[jobName];

            // Some minor validation
            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new ArgumentNullException($"No Quartz.NET Cron schedule found for job in configuration at {jobName}");
            }

            // register the job as before
            var jobKey = new JobKey(jobName);
            quartzConfigurator.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartzConfigurator.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule) // use the schedule from configuration
            );
        }
    }
}
