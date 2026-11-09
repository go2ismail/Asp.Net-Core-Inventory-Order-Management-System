using Infrastructure.SecurityManager.AspNetIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Infrastructure.SecurityManager.Tokens;

/// <summary>
/// Rebuilds the authenticated principal from the current database state on every request.
/// </summary>
/// <remarks>
/// A JWT is a statement about the past: it keeps the roles that were assigned when the token was
/// issued. Without this check, a deleted, blocked, or demoted account could keep using its token
/// until the token expires, and role claims carried only by a token would never be re-evaluated.
/// The roles of the principal are therefore replaced with the roles stored in ASP.NET Identity.
/// </remarks>
public static class LiveUserClaimsValidator
{
    public static async Task ValidateAsync(TokenValidatedContext context)
    {
        var principal = context.Principal;
        if (principal is null)
        {
            context.Fail("The access token does not contain a principal.");
            return;
        }

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            context.Fail("The access token does not contain a user identifier.");
            return;
        }

        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.FindByIdAsync(userId);

        if (user is null || user.IsDeleted == true)
        {
            context.Fail("The account that owns this access token no longer exists.");
            return;
        }

        if (user.IsBlocked == true)
        {
            context.Fail("The account that owns this access token is blocked.");
            return;
        }

        if (principal.Identity is not ClaimsIdentity identity)
        {
            context.Fail("The access token does not contain an identity.");
            return;
        }

        var currentRoles = await userManager.GetRolesAsync(user);

        foreach (var roleClaim in identity.FindAll(ClaimTypes.Role).ToList())
        {
            identity.RemoveClaim(roleClaim);
        }

        foreach (var role in currentRoles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        if (identity.FindFirst(ClaimTypes.Name) is null)
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName ?? user.Email ?? userId));
        }
    }
}
