using Domain.Common;

namespace Domain.Entities;

public class Product : BaseEntity
{
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? Description { get; set; }
    public double? UnitPrice { get; set; }
    public bool? Physical { get; set; } = true;
    public string? UnitMeasureId { get; set; }
    public UnitMeasure? UnitMeasure { get; set; }
    public string? ProductGroupId { get; set; }
    public ProductGroup? ProductGroup { get; set; }
}
