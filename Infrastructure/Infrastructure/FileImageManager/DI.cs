using Application.Common.Services.FileImageManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.FileImageManager;

public static class DI
{
    public static IServiceCollection RegisterFileImageManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileImageSettings>(configuration.GetSection("FileImageManager"));
        services.AddTransient<IFileImageService, FileImageService>();

        return services;
    }
}
