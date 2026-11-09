using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class TransferIn : BaseEntity
{
    public string? Number { get; set; }
    public DateTime? TransferReceiveDate { get; set; }
    public TransferStatus? Status { get; set; }
    public string? Description { get; set; }
    public string? TransferOutId { get; set; }
    public TransferOut? TransferOut { get; set; }
}
