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
        var jwtSectionName = "Jwt";
        services.Configure<TokenSettings>(configuration.GetSection(jwtSectionName));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var tokenSettings = configuration.GetSection(jwtSectionName).Get<TokenSettings>();

            if (tokenSettings?.Key.Length < 32)
            {
                throw new Exception("JWT key should be minimal 32 character");
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(tokenSettings?.ClockSkewInMinute ?? 5),
                ValidIssuer = tokenSettings?.Issuer,
                ValidAudience = tokenSettings?.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings?.Key ?? throw new ArgumentNullException("JWT Key is empty")))
            };

            options.Events = new JwtBearerEvents
            {
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

