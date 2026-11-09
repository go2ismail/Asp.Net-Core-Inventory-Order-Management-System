namespace Application.Common.Services.SecurityManager;

public record RefreshTokenResultDto
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
    public string? UserId { get; init; }
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? CompanyName { get; init; }
    public string? Avatar { get; init; }
    public List<MenuNavigationTreeNodeDto>? MenuNavigation { get; init; }
    public List<string>? Roles { get; init; }
}
