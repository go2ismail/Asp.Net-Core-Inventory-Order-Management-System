using Domain.Common;

namespace Domain.Entities;

public class TodoItem : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TodoId { get; set; }
    public Todo? Todo { get; set; }

}
