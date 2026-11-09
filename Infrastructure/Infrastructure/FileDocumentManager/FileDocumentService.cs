using Application.Common.Repositories;
using Application.Common.Services.FileDocumentManager;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Infrastructure.FileDocumentManager;

public class FileDocumentService : IFileDocumentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _folderPath;
    private readonly int _maxFileSizeInBytes;
    private readonly ICommandRepository<FileDocument> _docRepository;

    public FileDocumentService(
        IUnitOfWork unitOfWork,
        IOptions<FileDocumentSettings> settings,
        ICommandRepository<FileDocument> docRepository
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

        var doc = new FileDocument();
        doc.Name = fileName;
        doc.OriginalName = originalFileName;
        doc.Extension = docExtension;
        doc.GeneratedName = fileName;
        doc.FileSize = size;
        doc.Description = description;
        doc.CreatedById = createdById;

        await _docRepository.CreateAsync(doc, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return fileName;
    }

    public async Task<byte[]> GetFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(_folderPath, fileName);

        if (!File.Exists(filePath))
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "nodocument.txt");
        }

        var result = await File.ReadAllBytesAsync(filePath, cancellationToken);

        return result;
    }

}
