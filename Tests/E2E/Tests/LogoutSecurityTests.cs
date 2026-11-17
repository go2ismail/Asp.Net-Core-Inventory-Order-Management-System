using System.Net;
using System.Text.Json;
using Microsoft.Playwright;
using NUnit.Framework;

namespace E2E;

[TestFixture]
public class LogoutSecurityTests : ApiTestBase
{
    private const string VictimPassword = "E2e-Victim-Passw0rd";
    private const string AttackerPassword = "E2e-Attacker-Passw0rd";
    private const string FreshUserPassword = "E2e-Fresh-Passw0rd";

    [Test]
    public async Task Logout_RequiresAuthentication()
    {
        await using var context = await NewApiContextAsync(null);

        var response = await PostJsonAsync(context, "/api/Security/Logout", new
        {
            userId = Guid.NewGuid().ToString()
        });
        var body = await response.TextAsync();

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.Unauthorized),
            $"An anonymous logout must return 401 instead of succeeding. Body: {body}");

        using var document = JsonDocument.Parse(body);
        Assert.That(document.RootElement.GetProperty("code").GetInt32(), Is.EqualTo(401), $"The body code must be 401. Body: {body}");
    }

    [Test]
    public async Task Logout_DoesNotRevokeAnotherUsersTokens()
    {
        var admin = await LoginViaApiAsync(AdminEmail, AdminPassword);

        var victimEmail = $"e2e-logout-victim-{Guid.NewGuid():N}@example.com";
        var attackerEmail = $"e2e-logout-attacker-{Guid.NewGuid():N}@example.com";
        var victimFirstName = "E2E Logout Victim First";
        var victimLastName = "E2E Logout Victim Last";
        var victimCompanyName = "E2E Logout Victim Company";

        var victimId = await CreateUserViaApiAsync(
            admin.AccessToken,
            victimEmail,
            VictimPassword,
            victimFirstName,
            victimLastName,
            admin.UserId);
        await CreateUserViaApiAsync(
            admin.AccessToken,
            attackerEmail,
            AttackerPassword,
            "E2E Logout Attacker First",
            "E2E Logout Attacker Last",
            admin.UserId);

        var victim = await LoginViaApiAsync(victimEmail, VictimPassword);
        Assert.That(victim.UserId, Is.EqualTo(victimId));

        await using (var victimContext = await NewApiContextAsync(victim.AccessToken))
        {
            var updateResponse = await PostJsonAsync(victimContext, "/api/Security/UpdateMyProfile", new
            {
                userId = victim.UserId,
                firstName = victimFirstName,
                lastName = victimLastName,
                companyName = victimCompanyName
            });
            await AssertSuccessAsync(updateResponse, "The victim should be able to set the own company name.");
        }

        var attacker = await LoginViaApiAsync(attackerEmail, AttackerPassword);

        await using (var attackerContext = await NewApiContextAsync(attacker.AccessToken))
        {
            var response = await PostJsonAsync(attackerContext, "/api/Security/Logout", new
            {
                userId = victimId
            });
            var body = await response.TextAsync();

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK),
                $"The authenticated attacker should be able to log out the own session. Body: {body}");

            using var document = JsonDocument.Parse(body);
            Assert.That(document.RootElement.GetProperty("code").GetInt32(), Is.EqualTo(200), $"The logout should succeed. Body: {body}");

            Assert.That(body, Does.Not.Contain(victimEmail), "The response must not disclose the victim's email.");
            Assert.That(body, Does.Not.Contain(victimFirstName), "The response must not disclose the victim's first name.");
            Assert.That(body, Does.Not.Contain(victimLastName), "The response must not disclose the victim's last name.");
            Assert.That(body, Does.Not.Contain(victimCompanyName), "The response must not disclose the victim's company name.");
        }

        var victimRefresh = await RefreshTokenViaApiAsync(victim.RefreshToken);
        Assert.That(victimRefresh.Status, Is.EqualTo((int)HttpStatusCode.OK),
            "Logging out another account must not revoke the victim's refresh token.");
        Assert.That(victimRefresh.Code, Is.EqualTo(200), "The victim's refresh token must stay usable.");
        Assert.That(victimRefresh.RefreshToken, Is.Not.Null.And.Not.Empty, "The refresh should issue a new refresh token.");

        var attackerRefresh = await RefreshTokenViaApiAsync(attacker.RefreshToken);
        Assert.That(attackerRefresh.Code, Is.Not.EqualTo(200),
            "The logout must revoke the caller's own refresh token.");
        Assert.That(attackerRefresh.Message, Does.Contain("Refresh token invalid"),
            "The caller's old refresh token must be rejected after logout.");
    }

    [Test]
    public async Task Logout_RevokesOwnRefreshToken()
    {
        var admin = await LoginViaApiAsync(AdminEmail, AdminPassword);

        var email = $"e2e-logout-fresh-{Guid.NewGuid():N}@example.com";
        await CreateUserViaApiAsync(
            admin.AccessToken,
            email,
            FreshUserPassword,
            "E2E Logout Fresh First",
            "E2E Logout Fresh Last",
            admin.UserId);

        var user = await LoginViaApiAsync(email, FreshUserPassword);

        await using (var context = await NewApiContextAsync(user.AccessToken))
        {
            var response = await PostJsonAsync(context, "/api/Security/Logout", new { });
            await AssertSuccessAsync(response, "The user should be able to log out the own session.");
        }

        var refresh = await RefreshTokenViaApiAsync(user.RefreshToken);
        Assert.That(refresh.Code, Is.Not.EqualTo(200), "The logout must revoke the caller's refresh token.");
        Assert.That(refresh.Message, Does.Contain("Refresh token invalid"), "The old refresh token must be rejected after logout.");
    }

    [Test]
    public async Task LogoutPage_SignsOutAndClearsLocalStorage()
    {
        await LoginAsync();

        await Page.GotoAsync($"{BaseUrl}/Accounts/Logout", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        await Page.Locator("button[type=submit]").ClickAsync();

        await ExpectSwalTitleAsync("Logout Successful");
        await Page.WaitForURLAsync($"{BaseUrl}/", new PageWaitForURLOptions { Timeout = 20000 });

        await AssertStorageClearedAsync();
    }

    [Test]
    public async Task LogoutPage_ClearsLocalStateWhenSessionIsAlreadyInvalid()
    {
        await LoginAsync();

        await Page.EvaluateAsync("() => localStorage.clear()");
        await Context.ClearCookiesAsync();

        var logoutResponse = new TaskCompletionSource<IResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
        Page.Response += (_, response) =>
        {
            if (response.Url.Contains("/api/Security/Logout", StringComparison.OrdinalIgnoreCase))
            {
                logoutResponse.TrySetResult(response);
            }
        };

        await Page.GotoAsync($"{BaseUrl}/Accounts/Logout", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        await Page.Locator("button[type=submit]").ClickAsync();

        var response = await logoutResponse.Task.WaitAsync(TimeSpan.FromSeconds(30000));
        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.Unauthorized),
            "The logout request of an invalid session must be rejected with 401.");

        await Page.WaitForURLAsync($"{BaseUrl}/", new PageWaitForURLOptions { Timeout = 20000 });

        await AssertStorageClearedAsync();
    }

    private async Task AssertStorageClearedAsync()
    {
        var accessToken = await Page.EvaluateAsync<string?>("() => localStorage.getItem('accessToken')");
        var isAuthenticated = await Page.EvaluateAsync<string?>("() => localStorage.getItem('isAuthenticated')");

        Assert.That(accessToken, Is.Null, "localStorage.accessToken must be cleared after sign-out.");
        Assert.That(isAuthenticated, Is.Null, "localStorage.isAuthenticated must be cleared after sign-out.");
    }
}
