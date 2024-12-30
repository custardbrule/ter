using API.SSO.Domain;
using API.SSO.Infras.Context;
using API.SSO.Infras.Features.Behaviors;
using API.SSO.Infras.Services;
using API.SSO.Infras.Services.Implements;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace API.SSO.Infras
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureService(this IServiceCollection services, IConfiguration configuration)
        {
            // Register services
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IAuthService, AuthService>();

            // setup app context
            services.RegisterAppDbContext(configuration);

            // setup background Job
            services.RegisterQuartz(configuration);

            // Register Fluent Validator
            services.AddValidatorsFromAssembly(typeof(Configuration).Assembly);

            // Register Mediatr
            services.AddMediatR(cfg =>
            {
                cfg.AddOpenBehavior(typeof(LogBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidateBehavior<,>));
                cfg.RegisterServicesFromAssemblyContaining(typeof(Configuration));
            });

            services
                .AddAuthentication(cfg =>
                {
                    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                    };
                });

            return services;
        }

        internal static IServiceCollection RegisterAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<AppDbContext>(o =>
            {
                o.UseSqlServer(configuration.GetConnectionString("Development"), c => c.MigrationsAssembly("API.SSO"));
            });

            services
                .AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppDbContext>();

            return services;
        }
        internal static IServiceCollection RegisterQuartz(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(cfg =>
            {
                cfg.UsePersistentStore(o =>
                {
                    o.UseSqlServer(c =>
                    {
                        c.ConnectionString = configuration.GetRequiredSection("Quartz:ConnectionString").Value!;
                        c.TablePrefix = "QRTZ_";
                    });
                    o.UseNewtonsoftJsonSerializer();
                });

                cfg.UseSimpleTypeLoader();
                cfg.UseDefaultThreadPool(5);
                cfg.SchedulerName = "SSO_Scheduler";
            });

            services.AddQuartzHostedService(cfg =>
            {
                cfg.WaitForJobsToComplete = true;
                cfg.AwaitApplicationStarted = true;
            });

            // Register Job
            typeof(Configuration)
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IJob).IsAssignableFrom(t))
                .ToList()
                .ForEach(t => services.AddTransient(t));

            return services;
        }
    }
}
