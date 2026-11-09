using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class GoodsReceive : BaseEntity
{
    public string? Number { get; set; }
    public DateTime? ReceiveDate { get; set; }
    public GoodsReceiveStatus? Status { get; set; }
    public string? Description { get; set; }
    public string? PurchaseOrderId { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
}
