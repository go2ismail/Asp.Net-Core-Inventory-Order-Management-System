using ASPNET.BackEnd;
using ASPNET.BackEnd.Common.Configuration;
using ASPNET.BackEnd.Common.Middlewares;
using ASPNET.FrontEnd;

var builder = WebApplication.CreateBuilder(args);

//>>> Optional .env file (repository root or content root). It only fills the gaps, real
//>>> environment variables keep their precedence. See the "Configuration and Secrets" section in README.md.
var dotEnvFile = builder.Configuration.AddDotEnvFile(builder.Environment.ContentRootPath);

//>>> Create Logs folder for Serilog
var logPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app_data", "logs");
if (!Directory.Exists(logPath))
{
    Directory.CreateDirectory(logPath);
}

builder.Services.AddBackEndServices(builder.Configuration);
builder.Services.AddFrontEndServices();

var app = builder.Build();

if (builder.Configuration.GetValue<bool>("IsDemoVersion"))
{
    // The demo mode intentionally accepts the published credentials, the demo signing key, and the
    // demo seed data. Make that visible in every log so an accidental production deployment is noticed.
    app.Logger.LogWarning(
        "DEMO MODE IS ENABLED (IsDemoVersion=true). The published demo credentials and the demo JWT signing key are accepted, and demo data is seeded. Never use this mode with real data. Set IsDemoVersion=false and provide Jwt__Key and AspNetIdentity__DefaultAdmin__Password for a production deployment.");
}

if (dotEnvFile is not null)
{
    // Never log the values, only the file that was used, so operators can see where the secrets came from.
    app.Logger.LogInformation("Loaded configuration values from {DotEnvFile}", dotEnvFile);
    if (!app.Environment.IsDevelopment())
    {
        app.Logger.LogWarning(
            "A .env file is being used outside of the development environment. Prefer the environment variables of the host for deployments.");
    }
}

app.RegisterBackEndBuilder(app.Environment, app, builder.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseCors();
app.UseMiddleware<GlobalApiExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapFrontEndRoutes();
app.MapBackEndRoutes();

app.Run();
