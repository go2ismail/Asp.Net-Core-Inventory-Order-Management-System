namespace Infrastructure.FileImageManager;
public static class FileImageHelper
{
    private static readonly Dictionary<string, string> MimeTypes = new()
    {
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png", "image/png" },
        { ".gif", "image/gif" },
        { ".bmp", "image/bmp" },
    };

    public static string GetMimeType(string extension)
    {
        if (string.IsNullOrEmpty(extension))
            throw new Exception($"Extension cannot be null or empty: {nameof(extension)}");

        extension = extension.ToLowerInvariant();

        return MimeTypes.ContainsKey(extension)
            ? MimeTypes[extension]
            : "application/octet-stream";
    }
}
