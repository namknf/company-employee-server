using CompanyEmployees.ActionFilters;
using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Repository.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureIIS();
builder.Services.ConfigureLogging();
builder.Services.AddAuthorization();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();

builder.Services.AddControllers(conf =>
{
    conf.RespectBrowserAcceptHeader = true;
    conf.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters()
  .AddCustomCSVFormatter();

builder.Services.ConnectToDb(builder.Configuration);
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.ConfigureVersioning();

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateCompanyExistsAttribute>();
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
builder.Services.ConfigureDataShaper();

builder.Services.ConfigureSwagger();

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

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "CompanyEmployee v1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "CompanyEmployee v2");
});

app.Run();
