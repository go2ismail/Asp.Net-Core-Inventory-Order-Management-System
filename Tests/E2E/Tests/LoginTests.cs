using NUnit.Framework;

namespace E2E;

[TestFixture]
public class LoginTests : E2ETestBase
{
    [Test]
    public async Task ValidCredentials_RedirectToProfile()
    {
        await LoginAsync();
    }

    [Test]
    public async Task InvalidPassword_ShowsErrorAndStaysOnLoginPage()
    {
        await Page.GotoAsync($"{BaseUrl}/Accounts/Login", new Microsoft.Playwright.PageGotoOptions
        {
            WaitUntil = Microsoft.Playwright.WaitUntilState.DOMContentLoaded
        });

        await Page.Locator("#Email").FillAsync(AdminEmail);
        await Page.Locator("#Password").FillAsync("wrongpass1");
        await Page.Locator("button[type=submit]").ClickAsync();

        await Expect(Page.Locator(".swal2-popup")).ToBeVisibleAsync(new Microsoft.Playwright.LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 20000
        });
        await Expect(Page.Locator(".swal2-title")).ToHaveTextAsync(
            new System.Text.RegularExpressions.Regex("Login Failed|An Error Occurred"),
            new Microsoft.Playwright.LocatorAssertionsToHaveTextOptions { Timeout = 20000 });
        await Expect(Page.Locator(".swal2-icon.swal2-error")).ToBeVisibleAsync();

        Assert.That(Page.Url, Does.Contain("/Accounts/Login"));

        var isAuthenticated = await Page.EvaluateAsync<bool?>(
            "() => JSON.parse(localStorage.getItem('isAuthenticated'))");
        Assert.That(isAuthenticated, Is.Not.True);
    }
}
