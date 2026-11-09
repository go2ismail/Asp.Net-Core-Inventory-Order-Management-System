using NUnit.Framework;

namespace E2E;

[TestFixture]
public class UiSmokeTests : E2ETestBase
{
    [Test]
    public async Task HomePage_ShowsApplicationName()
    {
        await Page.GotoAsync(BaseUrl, new Microsoft.Playwright.PageGotoOptions
        {
            WaitUntil = Microsoft.Playwright.WaitUntilState.DOMContentLoaded
        });

        await Expect(Page.GetByText("INDOTALENT WHMS", new Microsoft.Playwright.PageGetByTextOptions
        {
            Exact = true
        })).ToBeVisibleAsync(new Microsoft.Playwright.LocatorAssertionsToBeVisibleOptions { Timeout = 20000 });
    }

    [Test]
    public async Task LoginPage_ShowsEmailAndPasswordForm()
    {
        await Page.GotoAsync($"{BaseUrl}/Accounts/Login", new Microsoft.Playwright.PageGotoOptions
        {
            WaitUntil = Microsoft.Playwright.WaitUntilState.DOMContentLoaded
        });

        await Expect(Page.Locator("#Email")).ToBeVisibleAsync();
        await Expect(Page.Locator("#Password")).ToBeVisibleAsync();
        await Expect(Page.Locator("button[type=submit]")).ToBeVisibleAsync();
    }

    [Test]
    public async Task ProfilePage_LoadsAfterLogin()
    {
        await LoginAsync();
    }

    [Test]
    public async Task WarehousePage_LoadsAfterLogin()
    {
        await LoginAsync();
        await GotoWarehouseListAsync();

        await Expect(Page.Locator(".sidebar a[href*='/Warehouses/WarehouseList']").First)
            .ToBeVisibleAsync(new Microsoft.Playwright.LocatorAssertionsToBeVisibleOptions { Timeout = 20000 });
    }

    [Test]
    public async Task DashboardPage_LoadsAfterLogin()
    {
        await LoginAsync();

        await Page.GotoAsync($"{BaseUrl}/Dashboards/DefaultDashboard", new Microsoft.Playwright.PageGotoOptions
        {
            WaitUntil = Microsoft.Playwright.WaitUntilState.DOMContentLoaded
        });

        await Expect(Page).ToHaveTitleAsync("Default Dashboard");
        await Expect(Page.Locator("#formcard")).ToBeVisibleAsync(new Microsoft.Playwright.LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 20000
        });
        await Expect(Page.GetByText("Sales", new Microsoft.Playwright.PageGetByTextOptions { Exact = true }).First)
            .ToBeVisibleAsync();
    }
}
