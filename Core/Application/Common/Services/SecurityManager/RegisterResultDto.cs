namespace Application.Common.Services.SecurityManager;

public record RegisterResultDto
{
    public string? UserId { get; init; }
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? CompanyName { get; init; }
}
