using ASPNET.BackEnd;
using ASPNET.BackEnd.Common.Middlewares;
using ASPNET.FrontEnd;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//>>> Configure Serilog logging
var logPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app_data", "logs");
try
{
    if (!Directory.Exists(logPath))
    {
        Directory.CreateDirectory(logPath);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to create logs directory: {ex.Message}");
}

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(Path.Combine(logPath, "app.log"), rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddBackEndServices(builder.Configuration);
builder.Services.AddFrontEndServices();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Register backend services and routes
app.RegisterBackEndBuilder(app.Environment, app, builder.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();

// Apply CORS policy
app.UseCors("AllowAll");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Global exception middleware should be after auth
app.UseMiddleware<GlobalApiExceptionHandlerMiddleware>();

// Map static assets and frontend/backend routes
app.MapStaticAssets();
app.MapFrontEndRoutes();
app.MapBackEndRoutes();

// Safe startup
try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}
