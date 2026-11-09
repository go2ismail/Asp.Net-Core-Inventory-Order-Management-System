using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class TransferOut : BaseEntity
{
    public string? Number { get; set; }
    public DateTime? TransferReleaseDate { get; set; }
    public TransferStatus? Status { get; set; }
    public string? Description { get; set; }
    public string? WarehouseFromId { get; set; }
    public Warehouse? WarehouseFrom { get; set; }
    public string? WarehouseToId { get; set; }
    public Warehouse? WarehouseTo { get; set; }
}
