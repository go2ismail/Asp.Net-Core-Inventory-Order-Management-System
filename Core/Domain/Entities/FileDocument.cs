using Domain.Common;

namespace Domain.Entities;
public class FileDocument : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? OriginalName { get; set; }
    public string? GeneratedName { get; set; }
    public string? Extension { get; set; }
    public long? FileSize { get; set; }
}
