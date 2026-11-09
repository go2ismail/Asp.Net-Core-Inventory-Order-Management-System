using Domain.Common;

namespace Domain.Entities;

public class NumberSequence : BaseEntity
{
    public string? EntityName { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public int? LastUsedCount { get; set; }
}

