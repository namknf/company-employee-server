using Contracts;
using LoggerService;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("Cors Policy", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });

        public static void ConfigureIIS(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = true;
                options.AuthenticationDisplayName = "SmbUser";
            });

        public static void ConfigureLogging(this IServiceCollection services) =>
            services.AddScoped<ILoggerService, LoggerManager>();
    }
}
