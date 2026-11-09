using Application.Common.Services.EmailManager;
using Application.Common.Services.SecurityManager;
using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Contexts;
using Infrastructure.SecurityManager.NavigationMenu;
using Infrastructure.SecurityManager.Roles;
using Infrastructure.SecurityManager.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;

using System.Data;
using System.Text;
using System.Text.Encodings.Web;
using static Domain.Common.Constants;

namespace Infrastructure.SecurityManager.AspNetIdentity;

public class SecurityService : ISecurityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly DataContext _context;
    private readonly IdentitySettings _identitySettings;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public SecurityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        DataContext context,
        IOptions<IdentitySettings> identitySettings,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
        _identitySettings = identitySettings.Value;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<LoginResultDto> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default
        )
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new Exception("Invalid login credentials.");
        }

        if (user.IsBlocked == true)
        {
            throw new Exception($"User is blocked. {email}");
        }

        if (user.IsDeleted == true)
        {
            throw new Exception($"User already deleted. {email}");
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, true, lockoutOnFailure: false);

        if (result.IsLockedOut)
        {
            throw new Exception("Invalid login credentials. IsLockedOut.");
        }

        if (!result.Succeeded)
        {
            throw new Exception("Invalid login credentials. NotSucceeded.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
        var accessToken = _tokenService.GenerateToken(user, roleClaims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var tokens = await _context.Token.Where(x => x.UserId == user.Id).ToListAsync(cancellationToken);
        foreach (var item in tokens)
        {
            _context.Remove(item);
        }

        var token = new Token();
        token.UserId = user.Id;
        token.RefreshToken = refreshToken;
        token.ExpiryDate = DateTime.UtcNow.AddDays(TokenConsts.ExpiryInDays);
        token.IsDeleted = false;
        token.CreatedAtUtc = DateTime.UtcNow;
        token.CreatedById = user.Id;
        await _context.AddAsync(token, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResultDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CompanyName = user.CompanyName,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            MenuNavigation = NavigationTreeStructure.GetCompleteMenuNavigationTreeNode(),
            Roles = roles.ToList(),
            Avatar = user.ProfilePictureName
        };
    }

    public async Task<LogoutResultDto> LogoutAsync(
        string userId,
        CancellationToken cancellationToken = default
        )
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var tokens = await _context.Token.Where(x => x.UserId == user.Id).ToListAsync(cancellationToken);
            foreach (var item in tokens)
            {
                _context.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        return new LogoutResultDto
        {
            UserId = user?.Id,
            Email = user?.Email,
            FirstName = user?.FirstName,
            LastName = user?.LastName,
            CompanyName = user?.CompanyName,
            UserClaims = null,
            AccessToken = null,
            RefreshToken = null,

        };
    }
    public async Task<RegisterResultDto> RegisterAsync(
        string email,
        string password,
        string confirmPassword,
        string firstName,
        string lastName,
        string companyName = "",
        CancellationToken cancellationToken = default
        )
    {
        if (!password.Equals(confirmPassword))
        {
            throw new Exception($"Password and ConfirmPassword is different.");
        }

        var user = new ApplicationUser(
            email,
            firstName,
            lastName,
            companyName
        );

        user.EmailConfirmed = !_identitySettings.SignIn.RequireConfirmedEmail;
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        if (!await _userManager.IsInRoleAsync(user, RoleHelper.GetProfileRole()))
        {
            await _userManager.AddToRoleAsync(user, RoleHelper.GetProfileRole());
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        if (_identitySettings.SignIn.RequireConfirmedEmail)
        {
            var request = _httpContextAccessor?.HttpContext?.Request;
            var callbackUrl = $"{request?.Scheme}://{request?.Host}/Accounts/EmailConfirm?email={user.Email}&code={code}";
            var encodeCallbackUrl = $"{HtmlEncoder.Default.Encode(callbackUrl)}";

            var emailSubject = $"Confirm your email";
            var emailMessage = $"Please confirm your account by <a href='{encodeCallbackUrl}'>clicking here</a>.";

            await _emailService.SendEmailAsync(user.Email ?? "", emailSubject, emailMessage);

        }

        return new RegisterResultDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CompanyName = user.CompanyName
        };
    }

    public async Task<string> ConfirmEmailAsync(
        string email,
        string code,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new Exception($"Unable to load user with email: {email}");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (!result.Succeeded)
        {
            throw new Exception($"Error confirming your email: {email}");
        }

        return email;
    }

    public async Task<string> ForgotPasswordAsync(
        string email,
        CancellationToken cancellationToken = default
        )
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new Exception($"Unable to load user with email: {email}");
        }

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var textTempPassword = Guid.NewGuid().ToString().Substring(0, _identitySettings.Password.RequiredLength);
        var encryptedTempPassword = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(textTempPassword));

        var request = _httpContextAccessor?.HttpContext?.Request;
        var callbackUrl = $"{request?.Scheme}://{request?.Host}/Accounts/ForgotPasswordConfirmation?email={user.Email}&code={code}&tempPassword={encryptedTempPassword}";
        var encodeCallbackUrl = $"{HtmlEncoder.Default.Encode(callbackUrl)}";

        var emailSubject = $"Forgot password confirmation";
        var emailMessage = $"Your temporary password is: <strong>{textTempPassword}</strong>. Please confirm reset your password by <a href='{encodeCallbackUrl}'>clicking here</a>.";

        await _emailService.SendEmailAsync(user.Email ?? "", emailSubject, emailMessage);

        return "A temporary password has been sent to the registered email address.";

    }

    public async Task<string> ForgotPasswordConfirmationAsync(
        string email,
        string tempPassword,
        string code,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new Exception($"Unable to load user with email: {email}");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        tempPassword = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(tempPassword));
        var result = await _userManager.ResetPasswordAsync(user, code, tempPassword);

        if (!result.Succeeded)
        {
            throw new Exception($"Error resetting your password");
        }

        return email;
    }

    public async Task<RefreshTokenResultDto> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken
        )
    {
        var registeredToken = await _context.Token.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);
        if (registeredToken == null)
        {
            throw new Exception("Refresh token invalid, please re-login");
        }
        var user = await _userManager.FindByIdAsync(registeredToken?.UserId ?? "");
        if (user == null)
        {
            throw new Exception("Refresh token invalid, please re-login");
        }
        _context.Token.Remove(registeredToken!);

        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
        var newAccessToken = _tokenService.GenerateToken(user, roleClaims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var token = new Token();
        token.UserId = user.Id;
        token.RefreshToken = newRefreshToken;
        token.ExpiryDate = DateTime.UtcNow.AddDays(TokenConsts.ExpiryInDays);
        token.IsDeleted = false;
        token.CreatedAtUtc = DateTime.UtcNow;
        token.CreatedById = user.Id;
        await _context.AddAsync(token, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResultDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CompanyName = user.CompanyName,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            MenuNavigation = NavigationTreeStructure.GetCompleteMenuNavigationTreeNode(),
            Roles = roles.ToList(),
            Avatar = user.ProfilePictureName
        };
    }

    public async Task<List<GetMyProfileListResultDto>> GetMyProfileListAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var profiles = await _context.Users
            .Where(x => x.Id == userId)
            .Select(x => new GetMyProfileListResultDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CompanyName = x.CompanyName
            })
            .ToListAsync(cancellationToken);

        return profiles;
    }

    public async Task UpdateMyProfileAsync(
        string userId,
        string firstName,
        string lastName,
        string companyName,
        CancellationToken cancellationToken
        )
    {
        var user = await _context.Users.Where(x => x.Id == userId).SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new Exception($"Unable to load user with id: {userId}");
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.CompanyName = companyName;

        _context.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task ChangePasswordAsync(
        string userId,
        string oldPassword,
        string newPassword,
        string confirmNewPassword,
        CancellationToken cancellationToken
    )
    {
        if (newPassword != confirmNewPassword)
        {
            throw new Exception("New password and confirm password do not match.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"Unable to load user with id: {userId}");
        }

        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Password change failed: {errors}");
        }

        var isDemoVersion = _configuration.GetValue<bool>("IsDemoVersion");
        if (isDemoVersion && user.Email == _identitySettings.DefaultAdmin.Email)
        {
            throw new Exception($"Update default admin password is not allowed in demo version.");
        }
    }

    public async Task<List<GetRoleListResultDto>> GetRoleListAsync(
        CancellationToken cancellationToken
    )
    {
        var roles = await _roleManager.Roles
            .Select(x => new GetRoleListResultDto
            {
                Id = x.Id,
                Name = x.Name ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        return roles;
    }

    public async Task<List<GetUserListResultDto>> GetUserListAsync(
        CancellationToken cancellationToken
        )
    {
        var users = await _userManager.Users
            .Select(x => new GetUserListResultDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                IsBlocked = x.IsBlocked,
                IsDeleted = x.IsDeleted,
                EmailConfirmed = x.EmailConfirmed,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<CreateUserResultDto> CreateUserAsync(
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
        )
    {
        if (!password.Equals(confirmPassword))
        {
            throw new Exception($"Password and ConfirmPassword is different.");
        }

        var user = new ApplicationUser(
            email,
            firstName,
            lastName
        );

        user.EmailConfirmed = emailConfirmed;
        user.IsBlocked = isBlocked;
        user.IsDeleted = isDeleted;
        user.CreatedById = createdById;

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        if (!await _userManager.IsInRoleAsync(user, RoleHelper.GetProfileRole()))
        {
            await _userManager.AddToRoleAsync(user, RoleHelper.GetProfileRole());
        }

        return new CreateUserResultDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailConfirmed = user.EmailConfirmed,
            IsBlocked = user.IsBlocked,
            IsDeleted = user.IsDeleted,
        };
    }

    public async Task<UpdateUserResultDto> UpdateUserAsync(
        string userId,
        string firstName,
        string lastName,
        bool emailConfirmed = true,
        bool isBlocked = false,
        bool isDeleted = false,
        string updatedById = "",
        CancellationToken cancellationToken = default
        )
    {
        var user = await _context.Users.Where(x => x.Id == userId).SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new Exception($"Unable to load user with id: {userId}");
        }

        if (user.Email == _identitySettings.DefaultAdmin.Email)
        {
            throw new Exception($"Update default admin is not allowed.");
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.EmailConfirmed = emailConfirmed;
        user.IsBlocked = isBlocked;
        user.IsDeleted = isDeleted;
        user.UpdatedById = updatedById;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return new UpdateUserResultDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailConfirmed = user.EmailConfirmed,
            IsBlocked = user.IsBlocked,
            IsDeleted = user.IsDeleted,
        };
    }

    public async Task<DeleteUserResultDto> DeleteUserAsync(
        string userId,
        string deletedById = "",
        CancellationToken cancellationToken = default
        )
    {
        var user = await _context.Users.Where(x => x.Id == userId).SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new Exception($"Unable to load user with id: {userId}");
        }

        if (user.Email == _identitySettings.DefaultAdmin.Email)
        {
            throw new Exception($"Update default admin is not allowed.");
        }

        user.IsDeleted = true;
        user.UpdatedById = deletedById;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return new DeleteUserResultDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailConfirmed = user.EmailConfirmed,
            IsBlocked = user.IsBlocked,
            IsDeleted = user.IsDeleted,
        };
    }

    public async Task UpdatePasswordUserAsync(
        string userId,
        string newPassword,
        CancellationToken cancellationToken
        )
    {

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"Unable to load user with id: {userId}");
        }

        var isDemoVersion = _configuration.GetValue<bool>("IsDemoVersion");
        if (isDemoVersion && user.Email == _identitySettings.DefaultAdmin.Email)
        {
            throw new Exception($"Update default admin password is not allowed in demo version.");
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Password change failed: {errors}");
        }
    }

    public async Task<List<string>> GetUserRolesAsync(
        string userId,
        CancellationToken cancellationToken = default
        )
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new Exception($"Unable to load user with id: {userId}");
        }

        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    public async Task<List<string>> UpdateUserRoleAsync(
            string userId,
            string roleName,
            bool accessGranted,
            CancellationToken cancellationToken = default
        )
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"Unable to load user with id: {userId}");
        }

        if (user.Email == _identitySettings.DefaultAdmin.Email)
        {
            throw new Exception($"Update default admin is not allowed.");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (accessGranted)
        {
            if (!currentRoles.Contains(roleName))
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to add role '{roleName}' to user with id: {userId}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
        else
        {
            if (currentRoles.Contains(roleName))
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to remove role '{roleName}' from user with id: {userId}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        var updatedRoles = await _userManager.GetRolesAsync(user);
        return updatedRoles.ToList();
    }



    public async Task ChangeAvatarAsync(
        string userId,
        string avatar,
        CancellationToken cancellationToken
        )
    {
        var user = await _context.Users.Where(x => x.Id == userId).SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new Exception($"Unable to load user with id: {userId}");
        }

        user.ProfilePictureName = avatar;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

}


