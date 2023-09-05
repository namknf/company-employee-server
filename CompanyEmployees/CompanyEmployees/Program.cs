using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
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
}).AddXmlDataContractSerializerFormatters()
  .AddCustomCSVFormatter();

builder.Services.ConnectToDb(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
