using System.Net;
using System.Text.Json;
using Microsoft.Playwright;
using NUnit.Framework;

namespace E2E;

[TestFixture]
public class ProfileAuthorizationTests : E2ETestBase
{
    private const string VictimPassword = "E2e-Victim-Passw0rd";
    private const string AttackerPassword = "E2e-Attacker-Passw0rd";

    [Test]
    public async Task ProfileEndpoints_RejectAnotherUsersId()
    {
        var admin = await LoginViaApiAsync(AdminEmail, AdminPassword);

        var victimEmail = $"e2e-profile-victim-{Guid.NewGuid():N}@example.com";
        var attackerEmail = $"e2e-profile-attacker-{Guid.NewGuid():N}@example.com";
        var victimFirstName = "E2E Victim First";
        var victimLastName = "E2E Victim Last";
        var victimCompanyName = "E2E Victim Company";

        var victimId = await CreateUserViaApiAsync(
            admin.AccessToken,
            victimEmail,
            VictimPassword,
            victimFirstName,
            victimLastName,
            admin.UserId);
        var attackerId = await CreateUserViaApiAsync(
            admin.AccessToken,
            attackerEmail,
            AttackerPassword,
            "E2E Attacker First",
            "E2E Attacker Last",
            admin.UserId);

        var victim = await LoginViaApiAsync(victimEmail, VictimPassword);
        Assert.That(victim.UserId, Is.EqualTo(victimId));

        await using (var victimContext = await NewApiContextAsync(victim.AccessToken))
        {
            var setupResponse = await PostJsonAsync(victimContext, "/api/Security/UpdateMyProfile", new
            {
                userId = victim.UserId,
                firstName = victimFirstName,
                lastName = victimLastName,
                companyName = victimCompanyName
            });
            await AssertSuccessAsync(setupResponse, "The victim should be able to update the own profile.");
        }

        var attacker = await LoginViaApiAsync(attackerEmail, AttackerPassword);
        Assert.That(attacker.UserId, Is.EqualTo(attackerId));

        await using (var attackerContext = await NewApiContextAsync(attacker.AccessToken))
        {
            var readResponse = await attackerContext.GetAsync($"/api/Security/GetMyProfileList?userId={victim.UserId}");
            await AssertForbiddenAsync(readResponse, "Reading another user's profile");
            var readBody = await readResponse.TextAsync();
            Assert.That(readBody, Does.Not.Contain(victimFirstName), "The response must not disclose the victim's first name.");
            Assert.That(readBody, Does.Not.Contain(victimLastName), "The response must not disclose the victim's last name.");
            Assert.That(readBody, Does.Not.Contain(victimCompanyName), "The response must not disclose the victim's company name.");

            var updateResponse = await PostJsonAsync(attackerContext, "/api/Security/UpdateMyProfile", new
            {
                userId = victim.UserId,
                firstName = "Hijacked First",
                lastName = "Hijacked Last",
                companyName = "Hijacked Company"
            });
            await AssertForbiddenAsync(updateResponse, "Updating another user's profile");

            var passwordResponse = await PostJsonAsync(attackerContext, "/api/Security/UpdateMyProfilePassword", new
            {
                userId = victim.UserId,
                oldPassword = VictimPassword,
                newPassword = "E2e-Hijacked-Passw0rd",
                confirmNewPassword = "E2e-Hijacked-Passw0rd"
            });
            await AssertForbiddenAsync(passwordResponse, "Changing another user's password");

            var avatarResponse = await PostJsonAsync(attackerContext, "/api/Security/UpdateMyProfileAvatar", new
            {
                userId = victim.UserId,
                avatar = "hijacked-avatar.png"
            });
            await AssertForbiddenAsync(avatarResponse, "Changing another user's avatar");
        }

        var victimAfterAttack = await LoginViaApiAsync(victimEmail, VictimPassword);
        Assert.That(victimAfterAttack.UserId, Is.EqualTo(victim.UserId));
        Assert.That(victimAfterAttack.Avatar, Is.Null.Or.Empty, "The victim's avatar must not have been changed.");

        await using (var victimContext = await NewApiContextAsync(victimAfterAttack.AccessToken))
        {
            var profileResponse = await victimContext.GetAsync($"/api/Security/GetMyProfileList?userId={victim.UserId}");
            var profile = await ReadOwnProfileAsync(profileResponse);
            Assert.That(profile.FirstName, Is.EqualTo(victimFirstName));
            Assert.That(profile.LastName, Is.EqualTo(victimLastName));
            Assert.That(profile.CompanyName, Is.EqualTo(victimCompanyName));

            var updateResponse = await PostJsonAsync(victimContext, "/api/Security/UpdateMyProfile", new
            {
                userId = victim.UserId,
                firstName = "E2E Victim Updated First",
                lastName = "E2E Victim Updated Last",
                companyName = "E2E Victim Updated Company"
            });
            await AssertSuccessAsync(updateResponse, "The victim should still be able to update the own profile.");

            var updatedResponse = await victimContext.GetAsync($"/api/Security/GetMyProfileList?userId={victim.UserId}");
            var updated = await ReadOwnProfileAsync(updatedResponse);
            Assert.That(updated.FirstName, Is.EqualTo("E2E Victim Updated First"));
            Assert.That(updated.LastName, Is.EqualTo("E2E Victim Updated Last"));
            Assert.That(updated.CompanyName, Is.EqualTo("E2E Victim Updated Company"));
        }
    }

    [Test]
    public async Task ProfileEndpoints_RequireAuthentication()
    {
        var userId = Guid.NewGuid().ToString();

        await using var context = await NewApiContextAsync(null);

        var readResponse = await context.GetAsync($"/api/Security/GetMyProfileList?userId={userId}");
        var readBody = await readResponse.TextAsync();
        Assert.That(readResponse.Status, Is.EqualTo((int)HttpStatusCode.Unauthorized),
            $"An anonymous profile read must return 401 instead of 403. Body: {readBody}");
        Assert.That(readResponse.Status, Is.Not.EqualTo((int)HttpStatusCode.Forbidden));
        using (var document = JsonDocument.Parse(readBody))
        {
            Assert.That(document.RootElement.GetProperty("code").GetInt32(), Is.EqualTo(401));
        }

        var updateResponse = await PostJsonAsync(context, "/api/Security/UpdateMyProfile", new
        {
            userId,
            firstName = "Anonymous",
            lastName = "Update",
            companyName = "Anonymous"
        });
        var updateBody = await updateResponse.TextAsync();
        Assert.That(updateResponse.Status, Is.EqualTo((int)HttpStatusCode.Unauthorized),
            $"An anonymous profile update must return 401 instead of 403. Body: {updateBody}");
        Assert.That(updateResponse.Status, Is.Not.EqualTo((int)HttpStatusCode.Forbidden));
    }

    private async Task<IAPIRequestContext> NewApiContextAsync(string? accessToken)
    {
        var headers = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(accessToken))
        {
            headers["Authorization"] = $"Bearer {accessToken}";
        }

        return await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = BaseUrl,
            ExtraHTTPHeaders = headers
        });
    }

    private static Task<IAPIResponse> PostJsonAsync(IAPIRequestContext context, string relativeUrl, object payload)
    {
        return context.PostAsync(relativeUrl, new APIRequestContextOptions
        {
            DataObject = payload
        });
    }

    private async Task<ApiSession> LoginViaApiAsync(string email, string password)
    {
        await using var context = await NewApiContextAsync(null);
        var response = await context.PostAsync("/api/Security/Login", new APIRequestContextOptions
        {
            DataObject = new { email, password }
        });
        var body = await response.TextAsync();

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK), $"The login for {email} should return 200. Body: {body}");

        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;
        Assert.That(root.GetProperty("code").GetInt32(), Is.EqualTo(200), $"The login for {email} should succeed. Body: {body}");

        var data = root.GetProperty("content").GetProperty("data");
        var userId = data.GetProperty("userId").GetString();
        var accessToken = data.GetProperty("accessToken").GetString();
        var avatar = data.TryGetProperty("avatar", out var avatarElement) && avatarElement.ValueKind == JsonValueKind.String
            ? avatarElement.GetString()
            : null;

        Assert.That(userId, Is.Not.Null.And.Not.Empty);
        Assert.That(accessToken, Is.Not.Null.And.Not.Empty);

        return new ApiSession(userId!, accessToken!, avatar);
    }

    private async Task<string> CreateUserViaApiAsync(
        string adminAccessToken,
        string email,
        string password,
        string firstName,
        string lastName,
        string createdById)
    {
        await using var context = await NewApiContextAsync(adminAccessToken);
        var response = await PostJsonAsync(context, "/api/Security/CreateUser", new
        {
            email,
            password,
            confirmPassword = password,
            firstName,
            lastName,
            emailConfirmed = true,
            isBlocked = false,
            isDeleted = false,
            createdById
        });
        var body = await response.TextAsync();

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK), $"Creating {email} should return 200. Body: {body}");

        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;
        Assert.That(root.GetProperty("code").GetInt32(), Is.EqualTo(200), $"Creating {email} should succeed. Body: {body}");

        var userId = root.GetProperty("content").GetProperty("data").GetProperty("userId").GetString();
        Assert.That(userId, Is.Not.Null.And.Not.Empty);

        return userId!;
    }

    private static async Task AssertSuccessAsync(IAPIResponse response, string scenario)
    {
        var body = await response.TextAsync();
        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK), $"{scenario} Body: {body}");

        using var document = JsonDocument.Parse(body);
        Assert.That(document.RootElement.GetProperty("code").GetInt32(), Is.EqualTo(200), $"{scenario} Body: {body}");
    }

    private static async Task AssertForbiddenAsync(IAPIResponse response, string scenario)
    {
        var body = await response.TextAsync();
        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.Forbidden), $"{scenario} should return 403. Body: {body}");

        using var document = JsonDocument.Parse(body);
        Assert.That(document.RootElement.GetProperty("code").GetInt32(), Is.EqualTo(403), $"{scenario} should report code 403. Body: {body}");
        Assert.That(document.RootElement.GetProperty("message").GetString(), Is.Not.Null.And.Not.Empty);
    }

    private static async Task<(string? FirstName, string? LastName, string? CompanyName)> ReadOwnProfileAsync(IAPIResponse response)
    {
        var body = await response.TextAsync();
        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK), $"Reading the own profile should return 200. Body: {body}");

        using var document = JsonDocument.Parse(body);
        var data = document.RootElement.GetProperty("content").GetProperty("data");
        Assert.That(data.GetArrayLength(), Is.GreaterThan(0), "The profile list should contain the current user.");

        var profile = data[0];
        return (
            profile.GetProperty("firstName").GetString(),
            profile.GetProperty("lastName").GetString(),
            profile.GetProperty("companyName").GetString());
    }

    private sealed record ApiSession(string UserId, string AccessToken, string? Avatar);
}
