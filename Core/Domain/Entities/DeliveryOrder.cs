using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class DeliveryOrder : BaseEntity
{
    public string? Number { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DeliveryOrderStatus? Status { get; set; }
    public string? Description { get; set; }
    public string? SalesOrderId { get; set; }
    public SalesOrder? SalesOrder { get; set; }
}
