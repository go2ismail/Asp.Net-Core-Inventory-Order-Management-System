namespace Domain.Common;

public class BaseEntity : IHasSequentialId, IHasIsDeleted, IHasAudit
{
    public string Id { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
    public string? CreatedById { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public string? UpdatedById { get; set; }

    public BaseEntity()
    {
        Id = GenerateSequentialGuid();
        IsDeleted = false;
    }



    private static readonly object _lock = new object();

    private string GenerateSequentialGuid()
    {
        byte[] guidArray = Guid.NewGuid().ToByteArray();

        DateTime baseDate = new DateTime(1900, 1, 1);
        DateTime now = DateTime.UtcNow;

        TimeSpan timeSpan = now - baseDate;
        byte[] daysArray = BitConverter.GetBytes(timeSpan.Days);
        byte[] msecsArray = BitConverter.GetBytes((long)(timeSpan.TotalMilliseconds % 86400000));

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);
        }

        lock (_lock)
        {
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
        }

        return new Guid(guidArray).ToString();
    }
}

