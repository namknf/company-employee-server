using CompanyEmployees.ActionFilters;
using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureIIS();
builder.Services.ConfigureLogging();
builder.Services.AddAuthorization();

builder.Services.AddControllers(conf =>
{
    conf.RespectBrowserAcceptHeader = true;
    conf.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters()
  .AddCustomCSVFormatter();

builder.Services.ConnectToDb(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateCompanyExistsAttribute>();
builder.Services.ConfigureDataShaper();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var nlogPath = Directory.GetCurrentDirectory() + "\\nlog.config";
LogManager.Setup().LoadConfigurationFromFile(nlogPath);

var app = builder.Build();

app.UseStaticFiles();
app.UseCors("Cors Policy");
app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
