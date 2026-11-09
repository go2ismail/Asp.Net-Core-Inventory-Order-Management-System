namespace Infrastructure.FileDocumentManager;
public class FileDocumentSettings
{
    public string PathFolder { get; set; } = string.Empty;
    public int MaxFileSizeInMB { get; set; }
}
