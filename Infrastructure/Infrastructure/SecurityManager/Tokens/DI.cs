using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Infrastructure.SecurityManager.Tokens;

public static class DI
{
    public static IServiceCollection RegisterToken(this IServiceCollection services, IConfiguration configuration)
    {
        // Demo mode keeps the documented demo key from appsettings.json; production requires a
        // deployment secret (environment variable, .env file, or user secrets) and rejects the demo
        // key, an empty key, and a short key. Validation runs here, so a bad configuration stops the start up.
        var isDemoVersion = configuration.GetValue<bool>("IsDemoVersion");
        var tokenSettings = TokenSettingsValidation.GetValidated(configuration, isDemoVersion);

        services.Configure<TokenSettings>(configuration.GetSection(TokenSettingsValidation.SectionName));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(tokenSettings.ClockSkewInMinute),
                ValidIssuer = tokenSettings.Issuer,
                ValidAudience = tokenSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Key))
            };

            options.Events = new JwtBearerEvents
            {
                // Authorization uses the current account and role state instead of the claims that
                // were baked into the token, see LiveUserClaimsValidator.
                OnTokenValidated = LiveUserClaimsValidator.ValidateAsync,

                // Prioritizing HttpOnly cookie before checking Authorization header
                OnMessageReceived = context =>
                {
                    var accessToken = context.HttpContext.Request.Cookies["accessToken"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }
                    else
                    {
                        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                        {
                            context.Token = authorizationHeader.Substring("Bearer ".Length).Trim();
                        }
                    }

                    return Task.CompletedTask;
                },

                // Custom handling for expired tokens
                OnChallenge = context =>
                {
                    if (context.AuthenticateFailure is SecurityTokenExpiredException)
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 498; // Custom status code for expired token
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new
                        {
                            code = 498,
                            message = "Token has expired.",
                            error = new
                            {
                                @ref = "https://datatracker.ietf.org/doc/html/rfc9110",
                                exceptionType = "SecurityTokenExpiredException",
                                innerException = "SecurityTokenExpiredException",
                                source = "",
                                stackTrace = ""
                            }
                        });

                        return context.Response.WriteAsync(result);
                    }

                    return Task.CompletedTask;
                }

            };

        });

        services.AddTransient<ITokenService, TokenService>();
        services.AddScoped<TokenSettings>();

        return services;
    }
}

