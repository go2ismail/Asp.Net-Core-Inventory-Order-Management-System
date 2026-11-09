using Application.Common.Repositories;
using Application.Common.Services.FileImageManager;
using Domain.Entities;
using Infrastructure.FileStorage;
using Microsoft.Extensions.Options;

namespace Infrastructure.FileImageManager;

public class FileImageService : IFileImageService
{
    private const string PlaceholderFileName = "noimage.png";

    private readonly IUnitOfWork _unitOfWork;
    private readonly string _folderPath;
    private readonly int _maxFileSizeInBytes;
    private readonly ICommandRepository<FileImage> _docRepository;

    public FileImageService(
        IUnitOfWork unitOfWork,
        IOptions<FileImageSettings> settings,
        ICommandRepository<FileImage> docRepository
        )
    {
        _unitOfWork = unitOfWork;
        _folderPath = Path.Combine(Directory.GetCurrentDirectory(), settings.Value.PathFolder);
        _maxFileSizeInBytes = settings.Value.MaxFileSizeInMB * 1024 * 1024;
        _docRepository = docRepository;
    }

    public async Task<string> UploadAsync(
        string? originalFileName,
        string? docExtension,
        byte[]? fileData,
        long? size,
        string? description = "",
        string? createdById = "",
        CancellationToken cancellationToken = default)
    {

        if (string.IsNullOrWhiteSpace(docExtension) || docExtension.Contains(Path.DirectorySeparatorChar) || docExtension.Contains(Path.AltDirectorySeparatorChar))
        {
            throw new Exception($"Invalid file extension: {nameof(docExtension)}");
        }

        if (fileData == null || fileData.Length == 0)
        {
            throw new Exception($"File data cannot be null or empty: {nameof(fileData)}");
        }

        if (fileData.Length > _maxFileSizeInBytes)
        {
            throw new Exception($"File size exceeds the maximum allowed size of {_maxFileSizeInBytes / (1024 * 1024)} MB");
        }

        var fileName = $"{Guid.NewGuid():N}.{docExtension}";

        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }

        var filePath = Path.Combine(_folderPath, fileName);

        await File.WriteAllBytesAsync(filePath, fileData, cancellationToken);

        var img = new FileImage();
        img.Name = fileName;
        img.OriginalName = originalFileName;
        img.Extension = docExtension;
        img.GeneratedName = fileName;
        img.FileSize = size;
        img.Description = description;
        img.CreatedById = createdById;

        await _docRepository.CreateAsync(img, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return fileName;
    }

    public async Task<byte[]> GetFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        // The name comes from the caller, so it is resolved through the guard: a value that tries to
        // leave the storage folder is treated as a missing image instead of a path to read.
        var filePath = StoragePathGuard.TryResolveInsideFolder(_folderPath, fileName, out var resolvedPath)
            && File.Exists(resolvedPath)
            ? resolvedPath
            : Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", PlaceholderFileName);

        var result = await File.ReadAllBytesAsync(filePath, cancellationToken);

        return result;
    }

}
