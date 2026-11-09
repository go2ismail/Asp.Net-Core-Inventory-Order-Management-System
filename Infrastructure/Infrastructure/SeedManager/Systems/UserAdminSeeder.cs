using Infrastructure.SecurityManager.AspNetIdentity;
using Infrastructure.SecurityManager.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.SeedManager.Systems;

public class UserAdminSeeder
{
    private readonly IdentitySettings _identitySettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserAdminSeeder> _logger;
    private readonly bool _isDemoVersion;

    public UserAdminSeeder(
        IOptions<IdentitySettings> identitySettings,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ILogger<UserAdminSeeder> logger
        )
    {
        _identitySettings = identitySettings.Value;
        _userManager = userManager;
        _logger = logger;
        _isDemoVersion = configuration.GetValue<bool>("IsDemoVersion");
    }

    public async Task GenerateDataAsync()
    {
        var adminEmail = DefaultAdminCredentialPolicy.EnsureEmail(
            _identitySettings.DefaultAdmin.Email,
            _isDemoVersion);
        var configuredPassword = _identitySettings.DefaultAdmin.Password;

        var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin is null)
        {
            await CreateAdminAsync(adminEmail, configuredPassword);
            return;
        }

        if (_isDemoVersion
            && (string.IsNullOrWhiteSpace(configuredPassword)
                || string.Equals(configuredPassword, DefaultAdminCredentialPolicy.PublishedDefaultPassword, StringComparison.Ordinal)))
        {
            // The live demo intentionally keeps the published credential. Rotating it would break the
            // documented admin@root.com / 123456 sign in that the demo page advertises.
            return;
        }

        // Production always rotates a published password. Demo mode only rotates when the deployment
        // deliberately configured a different password, which keeps the default demo sign in working.
        await RotatePublishedPasswordAsync(existingAdmin, adminEmail, configuredPassword);
    }

    private async Task CreateAdminAsync(string adminEmail, string? configuredPassword)
    {
        // The password is only required when the account has to be created, so an installation that
        // seeded its administrator before this setting existed keeps starting without it.
        var adminPassword = DefaultAdminCredentialPolicy.Validate(adminEmail, configuredPassword, _isDemoVersion);

        var applicationUser = new ApplicationUser(
            adminEmail,
            "Root",
            "Admin"
            );

        applicationUser.EmailConfirmed = true;

        //create user Root Admin
        var createResult = await _userManager.CreateAsync(applicationUser, adminPassword);
        if (!createResult.Succeeded)
        {
            throw ToException(createResult, $"Unable to create the administrator account {adminEmail}");
        }

        //add Admin role to Root Admin
        var roles = RoleHelper.GetAdminRoles();
        foreach (var role in roles)
        {
            if (!await _userManager.IsInRoleAsync(applicationUser, role))
            {
                var roleResult = await _userManager.AddToRoleAsync(applicationUser, role);
                if (!roleResult.Succeeded)
                {
                    throw ToException(roleResult, $"Unable to grant the role {role} to the administrator account {adminEmail}");
                }
            }

        }
    }

    private async Task RotatePublishedPasswordAsync(
        ApplicationUser admin,
        string adminEmail,
        string? configuredPassword)
    {
        // Installations that were seeded by an earlier release still carry the password that the
        // repository published. The check runs against the stored hash, so an administrator that
        // already chose another password is never touched.
        if (!await _userManager.CheckPasswordAsync(admin, DefaultAdminCredentialPolicy.PublishedDefaultPassword))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(configuredPassword))
        {
            throw new InvalidOperationException(
                DefaultAdminCredentialPolicy.BuildRotationRequiredMessage(adminEmail));
        }

        var newPassword = DefaultAdminCredentialPolicy.Validate(adminEmail, configuredPassword, _isDemoVersion);

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(admin);
        var resetResult = await _userManager.ResetPasswordAsync(admin, resetToken, newPassword);
        if (!resetResult.Succeeded)
        {
            throw ToException(resetResult, $"Unable to rotate the published default password of the administrator account {adminEmail}");
        }

        _logger.LogWarning(
            "The administrator account {Email} still used the password that earlier releases published. The password has been rotated to the configured value.",
            adminEmail);
    }

    private static Exception ToException(IdentityResult result, string context)
    {
        return new Exception($"{context}. Errors: {string.Join(", ", result.Errors.Select(error => error.Description))}");
    }
}
