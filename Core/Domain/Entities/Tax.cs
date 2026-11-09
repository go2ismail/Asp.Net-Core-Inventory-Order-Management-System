using Domain.Common;

namespace Domain.Entities;

public class Tax : BaseEntity
{
    public string? Name { get; set; }
    public double? Percentage { get; set; }
    public string? Description { get; set; }
}
