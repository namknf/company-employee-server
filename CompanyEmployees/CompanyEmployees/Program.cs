using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureIIS();
builder.Services.ConfigureLogging();

LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "/nlog.config");

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
