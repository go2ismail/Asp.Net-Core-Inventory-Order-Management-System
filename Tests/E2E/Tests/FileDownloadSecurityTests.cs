using System.Net;
using NUnit.Framework;

namespace E2E;

/// <summary>
/// Regression tests for the path traversal that was reported in issue #48: a caller supplied document
/// or image name must never be resolved outside the configured storage folder, so the download
/// endpoints must not return the content of appsettings.json or any other file on the server.
/// </summary>
[TestFixture]
public class FileDownloadSecurityTests : E2ETestBase
{
    [TestCase("/api/FileDocument/GetDocument?documentName=../../appsettings.json")]
    [TestCase("/api/FileDocument/GetDocument?documentName=..%2F..%2Fappsettings.json")]
    [TestCase("/api/FileDocument/GetDocument?documentName=..%5C..%5Cappsettings.json")]
    [TestCase("/api/FileDocument/GetDocument?documentName=C%3A%5CWindows%5Cwin.ini")]
    [TestCase("/api/FileDocument/GetDocument?documentName=%2Fetc%2Fpasswd")]
    [TestCase("/api/FileImage/GetImage?imageName=../../appsettings.json")]
    [TestCase("/api/FileImage/GetImage?imageName=..%2F..%2Fappsettings.json")]
    [TestCase("/api/FileImage/GetImage?imageName=..%5C..%5Cappsettings.json")]
    [TestCase("/api/FileImage/GetImage?imageName=C%3A%5CWindows%5Cwin.ini")]
    [TestCase("/api/FileImage/GetImage?imageName=%2Fetc%2Fpasswd")]
    public async Task FileNameThatEscapesTheStorageFolder_IsRejected(string relativeUrl)
    {
        await LoginAsync();

        var response = await GetWithAccessTokenAsync(relativeUrl);
        var body = await response.TextAsync();

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest),
            $"'{relativeUrl}' should be rejected with 400 Bad Request instead of being resolved on disk.");
        Assert.That(body, Does.Not.Contain("Jwt"), "The response must not expose configuration values.");
        Assert.That(body, Does.Not.Contain("[extensions]"), "The response must not expose a system file.");
    }

    [TestCase("/api/FileDocument/GetDocument")]
    [TestCase("/api/FileDocument/GetDocument?documentName=")]
    [TestCase("/api/FileImage/GetImage")]
    [TestCase("/api/FileImage/GetImage?imageName=")]
    public async Task MissingFileName_ServesThePlaceholderFile(string relativeUrl)
    {
        await LoginAsync();

        var response = await GetWithAccessTokenAsync(relativeUrl);

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK),
            $"'{relativeUrl}' should keep serving the placeholder file.");
    }
}
