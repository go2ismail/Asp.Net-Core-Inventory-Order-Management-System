using NUnit.Framework;

namespace E2E;

[TestFixture]
public class WarehouseCrudTests : E2ETestBase
{
    [SetUp]
    public async Task SignInAsync()
    {
        await LoginAsync();
        await GotoWarehouseListAsync();
    }

    [Test]
    public async Task AddForm_WithoutName_ShowsRequiredMessage()
    {
        await Page.Locator("#AddCustom").ClickAsync();
        await Expect(Page.Locator("#MainModal")).ToBeVisibleAsync();
        await SubmitModalAsync();

        await Expect(Page.Locator("#MainModal label.text-danger").Filter(new Microsoft.Playwright.LocatorFilterOptions
        {
            HasText = "Name is required."
        })).ToBeVisibleAsync(new Microsoft.Playwright.LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 20000
        });
        await Expect(Page.Locator("#MainModal")).ToBeVisibleAsync();

        await Page.Locator("#MainModal .modal-footer button[data-bs-dismiss=modal]").ClickAsync();
        await Expect(Page.Locator("#MainModal")).ToBeHiddenAsync();
    }

    [Test]
    public async Task Create_ShowsSuccessAlertAndNewGridRow()
    {
        var name = NewWarehouseName();

        await CreateWarehouseAsync(name);

        await DeleteWarehouseAsync(name);
    }

    [Test]
    public async Task Update_ChangesNameInGrid()
    {
        var name = NewWarehouseName();
        var updatedName = NewWarehouseName();

        await CreateWarehouseAsync(name);
        await UpdateWarehouseAsync(name, updatedName);

        await DeleteWarehouseAsync(updatedName);
    }

    [Test]
    public async Task Delete_RemovesRowFromGrid()
    {
        var name = NewWarehouseName();

        await CreateWarehouseAsync(name);
        await DeleteWarehouseAsync(name);
    }
}
