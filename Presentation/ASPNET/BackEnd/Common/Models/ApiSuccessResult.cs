namespace ASPNET.BackEnd.Common.Models;

public class ApiSuccessResult<T>
{
    public int? Code { get; init; }
    public string? Message { get; init; }
    public T? Content { get; init; }
}
