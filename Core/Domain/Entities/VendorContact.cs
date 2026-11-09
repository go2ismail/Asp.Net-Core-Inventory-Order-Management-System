using Domain.Common;

namespace Domain.Entities;

public class VendorContact : BaseEntity
{
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? Description { get; set; }
    public string? VendorId { get; set; }
    public Vendor? Vendor { get; set; }
}
