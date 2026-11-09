using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class StockCount : BaseEntity
{
    public string? Number { get; set; }
    public DateTime? CountDate { get; set; }
    public StockCountStatus? Status { get; set; }
    public string? Description { get; set; }
    public string? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
}
