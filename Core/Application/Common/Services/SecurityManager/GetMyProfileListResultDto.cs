namespace Application.Common.Services.SecurityManager;

public record GetMyProfileListResultDto
{
    public string? Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? CompanyName { get; init; }
}
