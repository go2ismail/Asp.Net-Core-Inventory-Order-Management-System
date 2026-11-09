using Domain.Common;

namespace Domain.Entities;

public class ProductGroup : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
