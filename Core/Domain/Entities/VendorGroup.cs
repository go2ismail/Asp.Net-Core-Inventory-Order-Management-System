using Domain.Common;

namespace Domain.Entities;

public class VendorGroup : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
