using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class PositiveAdjustment : BaseEntity
{
    public string? Number { get; set; }
    public DateTime? AdjustmentDate { get; set; }
    public AdjustmentStatus? Status { get; set; }
    public string? Description { get; set; }
}
