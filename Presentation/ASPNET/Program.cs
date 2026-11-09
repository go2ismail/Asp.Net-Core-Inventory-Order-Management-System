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
