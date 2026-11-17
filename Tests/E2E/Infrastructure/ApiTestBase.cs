using System.Net;
using System.Text.Json;
using Microsoft.Playwright;
using NUnit.Framework;

namespace E2E;

public abstract class ApiTestBase : E2ETestBase
{
    protected async Task<IAPIRequestContext> NewApiContextAsync(string? accessToken)
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

    protected static Task<IAPIResponse> PostJsonAsync(IAPIRequestContext context, string relativeUrl, object payload)
    {
        return context.PostAsync(relativeUrl, new APIRequestContextOptions
        {
            DataObject = payload
        });
    }

    protected async Task<ApiSession> LoginViaApiAsync(string email, string password)
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
        var refreshToken = data.GetProperty("refreshToken").GetString();
        var avatar = data.TryGetProperty("avatar", out var avatarElement) && avatarElement.ValueKind == JsonValueKind.String
            ? avatarElement.GetString()
            : null;

        Assert.That(userId, Is.Not.Null.And.Not.Empty);
        Assert.That(accessToken, Is.Not.Null.And.Not.Empty);
        Assert.That(refreshToken, Is.Not.Null.And.Not.Empty);

        return new ApiSession(userId!, accessToken!, refreshToken!, avatar);
    }

    protected async Task<string> CreateUserViaApiAsync(
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

    protected async Task<RefreshTokenAttempt> RefreshTokenViaApiAsync(string refreshToken)
    {
        await using var context = await NewApiContextAsync(null);
        var response = await PostJsonAsync(context, "/api/Security/RefreshToken", new { refreshToken });
        var body = await response.TextAsync();

        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;
        var code = root.TryGetProperty("code", out var codeElement) && codeElement.ValueKind == JsonValueKind.Number
            ? codeElement.GetInt32()
            : -1;
        var message = root.TryGetProperty("message", out var messageElement) && messageElement.ValueKind == JsonValueKind.String
            ? messageElement.GetString()
            : null;

        string? newRefreshToken = null;
        if (root.TryGetProperty("content", out var contentElement)
            && contentElement.ValueKind == JsonValueKind.Object
            && contentElement.TryGetProperty("data", out var dataElement)
            && dataElement.ValueKind == JsonValueKind.Object
            && dataElement.TryGetProperty("refreshToken", out var refreshElement)
            && refreshElement.ValueKind == JsonValueKind.String)
        {
            newRefreshToken = refreshElement.GetString();
        }

        return new RefreshTokenAttempt(response.Status, code, message, newRefreshToken);
    }

    protected static async Task AssertSuccessAsync(IAPIResponse response, string scenario)
    {
        var body = await response.TextAsync();
        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK), $"{scenario} Body: {body}");

        using var document = JsonDocument.Parse(body);
        Assert.That(document.RootElement.GetProperty("code").GetInt32(), Is.EqualTo(200), $"{scenario} Body: {body}");
    }

    protected static string? GetNullableString(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;
    }

    protected sealed record ApiSession(string UserId, string AccessToken, string RefreshToken, string? Avatar);

    protected sealed record RefreshTokenAttempt(int Status, int Code, string? Message, string? RefreshToken);
}
