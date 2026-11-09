namespace Application.Common.Services.SecurityManager;

public record LogoutResultDto
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
    public string? UserId { get; init; }
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? CompanyName { get; init; }
    public List<string>? UserClaims { get; init; }
}
