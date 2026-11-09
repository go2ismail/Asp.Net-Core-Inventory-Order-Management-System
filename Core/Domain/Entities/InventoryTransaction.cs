using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;


public class InventoryTransaction : BaseEntity
{
    public string? ModuleId { get; set; }
    public string? ModuleName { get; set; }
    public string? ModuleCode { get; set; }
    public string? ModuleNumber { get; set; }
    public DateTime? MovementDate { get; set; }
    public InventoryTransactionStatus? Status { get; set; }
    public string? Number { get; set; }
    public string? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    public string? ProductId { get; set; }
    public Product? Product { get; set; }
    public double? Movement { get; set; }
    public InventoryTransType? TransType { get; set; }
    public double? Stock { get; set; }
    public string? WarehouseFromId { get; set; }
    public Warehouse? WarehouseFrom { get; set; }
    public string? WarehouseToId { get; set; }
    public Warehouse? WarehouseTo { get; set; }
    public double? QtySCSys { get; set; }
    public double? QtySCCount { get; set; }
    public double? QtySCDelta { get; set; }

}
