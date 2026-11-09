namespace Infrastructure.FileDocumentManager;
public static class FileDocumentHelper
{
    private static readonly Dictionary<string, string> MimeTypes = new()
    {
        { ".pdf", "application/pdf" },
        { ".doc", "application/msword" },
        { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { ".xls", "application/vnd.ms-excel" },
        { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { ".txt", "text/plain" },
        { ".csv", "text/csv" },
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

