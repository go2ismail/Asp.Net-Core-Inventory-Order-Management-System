using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Scrapping : BaseEntity
{
    public string? Number { get; set; }
    public DateTime? ScrappingDate { get; set; }
    public ScrappingStatus? Status { get; set; }
    public string? Description { get; set; }
    public string? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
}
