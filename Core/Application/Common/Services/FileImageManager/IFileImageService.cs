namespace Application.Common.Services.FileImageManager;
public interface IFileImageService
{
    Task<string> UploadAsync(
        string? originalFileName,
        string? docExtension,
        byte[]? fileData,
        long? size,
        string? description = "",
        string? createdById = "",
        CancellationToken cancellationToken = default);
    Task<byte[]> GetFileAsync(string fileName, CancellationToken cancellationToken = default);
}
