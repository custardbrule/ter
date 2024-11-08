using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.AspNetCore;

namespace API.SSO.Infras
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureService(this IServiceCollection services, IConfiguration configuration)
        {
            // setup background Job
            services.AddQuartz(cfg =>
            {
                cfg.UsePersistentStore(o =>
                {
                    o.UseSqlServer(configuration.GetRequiredSection("Quartz:ConnectionString").Value!);
                    o.UseNewtonsoftJsonSerializer();
                });

                cfg.UseSimpleTypeLoader();
                cfg.UseDefaultThreadPool(5);
            });

            services.AddQuartzServer(cfg =>
            {
                cfg.WaitForJobsToComplete = true;
                cfg.AwaitApplicationStarted = true;
            });

            return services;
        }
    }
}
