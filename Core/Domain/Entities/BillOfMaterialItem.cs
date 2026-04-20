using Domain.Common;

namespace Domain.Entities;

public class BillOfMaterialItem : BaseEntity
{
    public string? BillOfMaterialId { get; set; }
    public BillOfMaterial? BillOfMaterial { get; set; }
    public string? ComponentProductId { get; set; }
    public Product? ComponentProduct { get; set; }
    public double? Quantity { get; set; } = 1;
    public int? Sequence { get; set; }
    public string? UnitMeasureId { get; set; }
    public UnitMeasure? UnitMeasure { get; set; }
    public double? ScrapPercentage { get; set; } = 0;
    public string? Notes { get; set; }
}
