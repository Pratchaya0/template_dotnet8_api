using Quartz;
using template_dotnet8_api.Configurations;
using template_dotnet8_api.HostedServices;
using template_dotnet8_api.Startups;

namespace template_dotnet8_api
{
    public static class ProjectSetup
    {
        /// <summary>
        /// ใส่ Dependency Injection ที่ใช้ใน Project
        /// </summary>
        public static IServiceCollection ConfigDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // TODO: ใช้สำหรับข้อมูล Login
            // services.AddScoped<ILoginDetailServices, LoginDetailServices>();

            // TODO: เมื่อเขียน Service และ Interface ของ Service ให้ใส่ที่นี้
            // services.AddSingleton // ใช้เมื่อใช้ Instance เดียวทั้ง Project
            // services.AddScoped // ใช้เมื่อแยก Instance ตาม User
            // services.AddTransient // ใช้เมื่อสร้าง Instance ใหม่ทุกครั้งที่เรียกใช้

            // TODO: ตัวอย่างการเขียน RestSharp หากไม่ใช้ให้ลบ Folder Examples ทิ้ง
            // services.Configure<ServiceURL>(configuration.GetSection("ServiceURL"));
            // services.AddSingleton<ShortLinkClient>();
            // services.AddSingleton<SendSmsClient>();
            // วิธีการเขียน RestSharp
            // https://github.com/SiamsmileDev/DevKnowledgeBase/blob/develop/Example%20Code/CSharp/RestSharp%20Example.md
            // TODO: เปลียน Link

            return services;
        }

        /// <summary>
        /// ใส่ Job Schedule(Quartz) ที่ใช้ใน Project
        /// </summary>
        public static IServiceCollectionQuartzConfigurator ConfigQuartz(this IServiceCollectionQuartzConfigurator quartz, QuartzSetting quartzSetting)
        {
            // TODO: เมื่อเขียน Job Schedule แล้วให้ใส่งานที่นี้ ให้เพิ่ม Schedule ใน appsetting.json ด้วย
            // การสร้าง Job Schedule ดูได้ที่
            // https://github.com/SiamsmileDev/DevKnowledgeBase/blob/develop/Example%20Code/CSharp/Quartz%20Job%20Scheduling.md
            // TODO: แก้ Link

            // Job สำหรับการลบ Log ใน Database
            quartz.AddJobAndTrigger<LoggerRetentionJob>(quartzSetting);

            return quartz;
        }
    }
}
