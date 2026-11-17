using System.Net;
using System.Text.Json;
using NUnit.Framework;

namespace E2E;

[TestFixture]
public class ExceptionHandlingTests : ApiTestBase
{
    [Test]
    public async Task UnhandledException_ReturnsHttp500WithoutInternals()
    {
        await using var context = await NewApiContextAsync(null);

        var response = await PostJsonAsync(context, "/api/Security/RefreshToken", new
        {
            refreshToken = "e2e-invalid-refresh-token"
        });
        var body = await response.TextAsync();

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.InternalServerError),
            $"An unhandled exception must return HTTP 500 instead of reporting it only in the body. Body: {body}");

        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;

        Assert.That(root.GetProperty("code").GetInt32(), Is.EqualTo(500), $"The body code must be 500. Body: {body}");
        Assert.That(root.GetProperty("message").GetString(), Is.Not.Null.And.Not.Empty, "The error message must stay available to existing clients.");

        var error = root.GetProperty("error");
        Assert.That(GetNullableString(error, "stackTrace"), Is.Null, "The response must not serialize a stack trace.");
        Assert.That(GetNullableString(error, "source"), Is.Null, "The response must not serialize the exception source.");
        Assert.That(GetNullableString(error, "innerException"), Is.Null, "The response must not serialize the inner exception.");
        Assert.That(GetNullableString(error, "exceptionType"), Is.Not.Null.And.Not.Empty, "The exception type name should still be reported.");

        Assert.That(body, Does.Not.Contain(".cs:line"), "The response must not contain file and line information.");
        Assert.That(body, Does.Not.Contain("   at "), "The response must not contain stack frames.");
        Assert.That(body, Does.Not.Contain("CustomExceptionHandler"), "The response must not expose server internals.");
    }

    [Test]
    public async Task AuthenticationFailure_KeepsHttp401()
    {
        var userId = Guid.NewGuid().ToString();

        await using var context = await NewApiContextAsync(null);

        var response = await context.GetAsync($"/api/Security/GetMyProfileList?userId={userId}");
        var body = await response.TextAsync();

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.Unauthorized),
            $"An anonymous request must stay 401 when the handler runs. Body: {body}");

        using var document = JsonDocument.Parse(body);
        Assert.That(document.RootElement.GetProperty("code").GetInt32(), Is.EqualTo(401), $"The body code must stay 401. Body: {body}");
    }
}
