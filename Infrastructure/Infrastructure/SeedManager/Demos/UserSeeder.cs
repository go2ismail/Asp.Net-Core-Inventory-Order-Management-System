using Infrastructure.SecurityManager.AspNetIdentity;
using Infrastructure.SecurityManager.Roles;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.SeedManager.Demos;

public class UserSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserSeeder(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GenerateDataAsync()
    {
        var userNames = new List<string>
        {
            "Alex", "Taylor", "Jordan", "Morgan", "Riley",
            "Casey", "Peyton", "Cameron", "Jamie", "Drew",
            "Dakota", "Avery", "Quinn", "Harper", "Rowan",
            "Emerson", "Finley", "Skyler", "Charlie", "Sage"
        };

        var defaultPassword = "123456";
        var domain = "@example.com";

        foreach (var name in userNames)
        {
            var email = $"{name.ToLower()}{domain}";

            if (await _userManager.FindByEmailAsync(email) == null)
            {
                var applicationUser = new ApplicationUser(email, name, "User")
                {
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(applicationUser, defaultPassword);

                var role = RoleHelper.GetProfileRole();
                if (!await _userManager.IsInRoleAsync(applicationUser, role))
                {
                    await _userManager.AddToRoleAsync(applicationUser, role);
                }
            }
        }
    }
}
