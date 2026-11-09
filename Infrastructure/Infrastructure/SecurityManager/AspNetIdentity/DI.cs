using Application.Common.Services.SecurityManager;
using Infrastructure.DataAccessManager.EFCore.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SecurityManager.AspNetIdentity;

public static class DI
{
    public static IServiceCollection RegisterSecurityManager(this IServiceCollection services, IConfiguration configuration)
    {
        var aspNetIdentitySectionName = "AspNetIdentity";

        services.Configure<IdentitySettings>(configuration.GetSection(aspNetIdentitySectionName));

        services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            var identitySettings = configuration.GetSection(aspNetIdentitySectionName).Get<IdentitySettings>();
            if (identitySettings == null)
            {
                throw new Exception($"{aspNetIdentitySectionName} configuration section at appsettings.json is missing");
            }

            // Password settings
            options.Password.RequireDigit = identitySettings.Password.RequireDigit;
            options.Password.RequireLowercase = identitySettings.Password.RequireLowercase;
            options.Password.RequireUppercase = identitySettings.Password.RequireUppercase;
            options.Password.RequireNonAlphanumeric = identitySettings.Password.RequireNonAlphanumeric;
            options.Password.RequiredLength = identitySettings.Password.RequiredLength;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.Lockout.DefaultLockoutTimeSpanInMinutes);
            options.Lockout.MaxFailedAccessAttempts = identitySettings.Lockout.MaxFailedAccessAttempts;
            options.Lockout.AllowedForNewUsers = identitySettings.Lockout.AllowedForNewUsers;

            // User settings
            options.User.RequireUniqueEmail = identitySettings.User.RequireUniqueEmail;

            // SignIn settings
            options.SignIn.RequireConfirmedEmail = identitySettings.SignIn.RequireConfirmedEmail;

        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>();

        services.AddScoped<ISecurityService, SecurityService>();

        return services;
    }

}


