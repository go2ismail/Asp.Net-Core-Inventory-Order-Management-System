using Domain.Common;

namespace Domain.Entities;

public class BillOfMaterial : BaseEntity
{
    public string? ProductId { get; set; }
    public Product? Product { get; set; }
    public string? Name { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; } = true;
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public ICollection<BillOfMaterialItem>? Items { get; set; }
}
