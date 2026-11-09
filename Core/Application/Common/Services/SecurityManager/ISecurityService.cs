namespace Application.Common.Services.SecurityManager;

public interface ISecurityService
{
    public Task<LoginResultDto> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default
        );

    public Task<LogoutResultDto> LogoutAsync(
        string userId,
        CancellationToken cancellationToken = default
        );

    public Task<RegisterResultDto> RegisterAsync(
        string email,
        string password,
        string confirmPassword,
        string firstName,
        string lastName,
        string companyName = "",
        CancellationToken cancellationToken = default
        );

    public Task<string> ConfirmEmailAsync(
        string email,
        string code,
        CancellationToken cancellationToken = default
        );

    public Task<string> ForgotPasswordAsync(
        string email,
        CancellationToken cancellationToken = default
        );

    public Task<string> ForgotPasswordConfirmationAsync(
        string email,
        string tempPassword,
        string code,
        CancellationToken cancellationToken = default
        );

    public Task<RefreshTokenResultDto> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken
        );

    public Task<List<GetMyProfileListResultDto>> GetMyProfileListAsync(
        string userId,
        CancellationToken cancellationToken
        );

    public Task UpdateMyProfileAsync(
        string userId,
        string firstName,
        string lastName,
        string companyName,
        CancellationToken cancellationToken
        );

    public Task ChangePasswordAsync(
        string userId,
        string oldPassword,
        string newPassword,
        string confirmNewPassword,
        CancellationToken cancellationToken
        );

    public Task<List<GetRoleListResultDto>> GetRoleListAsync(
        CancellationToken cancellationToken
        );

    public Task<List<GetUserListResultDto>> GetUserListAsync(
        CancellationToken cancellationToken
        );

    public Task<CreateUserResultDto> CreateUserAsync(
        string email,
        string password,
        string confirmPassword,
        string firstName,
        string lastName,
        bool emailConfirmed = true,
        bool isBlocked = false,
        bool isDeleted = false,
        string createdById = "",
        CancellationToken cancellationToken = default
        );

    public Task<UpdateUserResultDto> UpdateUserAsync(
        string userId,
        string firstName,
        string lastName,
        bool emailConfirmed = true,
        bool isBlocked = false,
        bool isDeleted = false,
        string updatedById = "",
        CancellationToken cancellationToken = default
        );

    public Task<DeleteUserResultDto> DeleteUserAsync(
        string userId,
        string deletedById = "",
        CancellationToken cancellationToken = default
        );

    public Task UpdatePasswordUserAsync(
        string userId,
        string newPassword,
        CancellationToken cancellationToken
        );

    public Task<List<string>> GetUserRolesAsync(
        string userId,
        CancellationToken cancellationToken = default
        );

    public Task<List<string>> UpdateUserRoleAsync(
        string userId,
        string roleName,
        bool accessGranted,
        CancellationToken cancellationToken = default
        );

    public Task ChangeAvatarAsync(
        string userId,
        string avatar,
        CancellationToken cancellationToken
        );
}
