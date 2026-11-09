using Application.Common.Services.FileDocumentManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.FileDocumentManager;

public static class DI
{
    public static IServiceCollection RegisterFileDocumentManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileDocumentSettings>(configuration.GetSection("FileDocumentManager"));
        services.AddTransient<IFileDocumentService, FileDocumentService>();

        return services;
    }
}
