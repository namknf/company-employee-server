using Contracts;
using Entities;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.DataShaping;

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

        public static void ConnectToDb(this IServiceCollection services, IConfiguration config) =>
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(config.GetConnectionString("connect"), optAct => optAct.MigrationsAssembly("CompanyEmployees")));

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureDataShaper(this IServiceCollection services) =>
            services.AddScoped(typeof(IDataShaper<>), typeof(DataShaper<>));

        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
            builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
    }
}
