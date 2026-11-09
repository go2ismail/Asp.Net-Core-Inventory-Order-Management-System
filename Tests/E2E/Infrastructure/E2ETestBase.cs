using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace E2E;

public abstract class E2ETestBase : PageTest
{
    protected const string AdminEmail = "admin@root.com";
    protected const string AdminPassword = "123456";

    protected static string BaseUrl => E2EServerFixture.BaseUrl;

    protected ILocator NameInput => Page.Locator("#MainModal input.e-input").First;

    protected ILocator WarehouseRows => Page.Locator(".e-grid .e-row");

    [SetUp]
    public void ConfigureTimeouts()
    {
        Page.SetDefaultTimeout(30000);
        Page.SetDefaultNavigationTimeout(60000);
        Context.SetDefaultTimeout(30000);
    }

    [TearDown]
    public async Task CaptureFailureScreenshotAsync()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
        {
            return;
        }

        try
        {
            var directory = Path.Combine(E2EServerFixture.RepoRoot, "TestResults", "screenshots");
            Directory.CreateDirectory(directory);
            var testName = string.Join("_", TestContext.CurrentContext.Test.Name.Split(Path.GetInvalidFileNameChars()));
            var path = Path.Combine(directory, $"{testName}-{DateTime.UtcNow:yyyyMMddHHmmss}.png");
            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = path, FullPage = true });
        }
        catch
        {
            // A screenshot failure must not hide the original test failure.
        }
    }

    protected async Task LoginAsync()
    {
        await Page.GotoAsync($"{BaseUrl}/Accounts/Login", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        await Page.Locator("#Email").FillAsync(AdminEmail);
        await Page.Locator("#Password").FillAsync(AdminPassword);
        await Page.Locator("button[type=submit]").ClickAsync();

        await Page.WaitForURLAsync("**/Profiles/MyProfile", new PageWaitForURLOptions
        {
            Timeout = 20000
        });

        await Expect(Page).ToHaveTitleAsync("My Profile", new PageAssertionsToHaveTitleOptions
        {
            Timeout = 20000
        });

        var isAuthenticated = await Page.EvaluateAsync<bool?>(
            "() => JSON.parse(localStorage.getItem('isAuthenticated'))");
        Assert.That(isAuthenticated, Is.True, "localStorage.isAuthenticated should be true after login.");

        await Expect(Page.Locator("#userEmail")).ToHaveTextAsync(AdminEmail);
    }

    protected async Task GotoWarehouseListAsync()
    {
        await Page.GotoAsync($"{BaseUrl}/Warehouses/WarehouseList", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        await Expect(Page).ToHaveTitleAsync("Warehouse List");
        await Expect(Page.Locator("#AddCustom")).ToBeVisibleAsync();
        await Expect(WarehouseRows.First).ToBeVisibleAsync();
    }

    protected static string NewWarehouseName() => $"E2E-WH-{Guid.NewGuid():N}";

    protected ILocator WarehouseRow(string name) =>
        WarehouseRows.Filter(new LocatorFilterOptions { HasText = name });

    protected async Task FillWarehouseFormAsync(string name, string? description = null)
    {
        await Expect(Page.Locator("#MainModal")).ToBeVisibleAsync();
        await NameInput.FillAsync(name);
        await Page.Locator("#Description").FillAsync(description ?? $"E2E description for {name}");
    }

    protected async Task SubmitModalAsync()
    {
        await Page.Locator("#MainSaveButton").ClickAsync();
    }

    protected async Task ExpectSwalTitleAsync(string title)
    {
        await Expect(Page.Locator(".swal2-title")).ToHaveTextAsync(title, new LocatorAssertionsToHaveTextOptions
        {
            Timeout = 20000
        });
    }

    protected async Task ExpectModalClosedAsync()
    {
        await Expect(Page.Locator("#MainModal")).ToBeHiddenAsync(new LocatorAssertionsToBeHiddenOptions
        {
            Timeout = 20000
        });
        await Expect(Page.Locator(".swal2-container")).ToHaveCountAsync(0, new LocatorAssertionsToHaveCountOptions
        {
            Timeout = 20000
        });
    }

    protected async Task SelectWarehouseRowAsync(string name)
    {
        var row = WarehouseRow(name).First;
        await Expect(row).ToBeVisibleAsync();

        if (await row.GetAttributeAsync("aria-selected") != "true")
        {
            await row.GetByText(name, new LocatorGetByTextOptions { Exact = true }).ClickAsync();
        }

        await Expect(row).ToHaveAttributeAsync("aria-selected", "true", new LocatorAssertionsToHaveAttributeOptions
        {
            Timeout = 10000
        });
        await Expect(Page.Locator("#EditCustom")).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions
        {
            Timeout = 10000
        });
    }

    protected async Task CreateWarehouseAsync(string name)
    {
        await Page.Locator("#AddCustom").ClickAsync();
        await FillWarehouseFormAsync(name);
        await SubmitModalAsync();
        await ExpectSwalTitleAsync("Save Successful");
        await ExpectModalClosedAsync();
        await Expect(WarehouseRow(name).First).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 20000
        });
    }

    protected async Task UpdateWarehouseAsync(string oldName, string newName)
    {
        await SelectWarehouseRowAsync(oldName);
        await Page.Locator("#EditCustom").ClickAsync();
        await FillWarehouseFormAsync(newName);
        await SubmitModalAsync();
        await ExpectSwalTitleAsync("Save Successful");
        await ExpectModalClosedAsync();
        await Expect(WarehouseRow(newName).First).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 20000
        });
        await Expect(WarehouseRow(oldName)).ToHaveCountAsync(0);
    }

    protected async Task DeleteWarehouseAsync(string name)
    {
        await SelectWarehouseRowAsync(name);
        await Page.Locator("#DeleteCustom").ClickAsync();
        await Expect(Page.Locator("#MainModal")).ToBeVisibleAsync();
        await SubmitModalAsync();
        await ExpectSwalTitleAsync("Delete Successful");
        await ExpectModalClosedAsync();
        await Expect(WarehouseRow(name)).ToHaveCountAsync(0, new LocatorAssertionsToHaveCountOptions
        {
            Timeout = 20000
        });
    }
}



