using Application;
using ASPNET.BackEnd.Common.Handlers;
using Infrastructure;
using Infrastructure.DataAccessManager.EFCore;
using Infrastructure.SeedManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace ASPNET.BackEnd;

public static class BackEndConfiguration
{
    public static IServiceCollection AddBackEndServices(this IServiceCollection services, IConfiguration configuration)
    {
        //>>> Application Layer
        services.AddApplicationServices();

        //>>> Infrastructure Layer
        services.AddInfrastructureServices(configuration);

        services.AddExceptionHandler<CustomExceptionHandler>();

        //>>> Common

        services.AddHttpContextAccessor();
        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Indotalent API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

        });


        services.Configure<ApiBehaviorOptions>(x =>
        {
            x.SuppressModelStateInvalidFilter = true;
        });

        return services;
    }

    public static IEndpointRouteBuilder MapBackEndRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllers();

        return endpoints;
    }

    public static IApplicationBuilder RegisterBackEndBuilder(
        this IApplicationBuilder app,
        IWebHostEnvironment environment,
        IHost host,
        IConfiguration configuration
        )
    {
        // >>> Create database
        host.CreateDatabase();

        //seed database with system data
        host.SeedSystemData();

        //seed database with demo data
        if (configuration.GetValue<bool>("IsDemoVersion"))
        {
            host.SeedDemoData();
        }

        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Indotalent V1");
            });
        }

        return app;
    }


}
