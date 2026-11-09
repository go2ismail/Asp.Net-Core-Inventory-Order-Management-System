using NUnit.Framework;

namespace E2E;

[TestFixture]
public class VendorCrudTests : CrudModuleTestBase
{
    // Seeded by Infrastructure/SeedManager/Demos/VendorGroupSeeder.cs and VendorCategorySeeder.cs.
    private const string GroupName = "Manufacture";
    private const string CategoryName = "Specialty";

    protected override string ListPath => "/Vendors/VendorList";

    protected override string ListTitle => "Vendor List";

    protected override string NamePrefix => "E2E-VEN";

    [SetUp]
    public async Task SignInAsync()
    {
        await LoginAsync();
        await OpenListAsync();
    }

    protected override async Task FillFormAsync(string name)
    {
        await FillTextBoxAsync("Name", name);
        await SelectDropDownAsync("VendorGroupId", GroupName);
        await SelectDropDownAsync("VendorCategoryId", CategoryName);
        await FillTextBoxAsync("Description", $"E2E vendor {name}");
        await FillTextBoxAsync("Street", "Jl. E2E No. 2");
        await FillTextBoxAsync("City", "Jakarta");
        await FillTextBoxAsync("State", "DKI Jakarta");
        await FillTextBoxAsync("ZipCode", "10110");
        await FillTextBoxAsync("Country", "Indonesia");
        await FillTextBoxAsync("PhoneNumber", "+62 21 7654321");
        await FillTextBoxAsync("EmailAddress", $"{name.ToLowerInvariant()}@example.com");
    }

    [Test]
    public async Task AddForm_WithoutName_ShowsRequiredMessage()
    {
        await ExpectNameRequiredMessageAsync();
    }

    [Test]
    public async Task Create_ShowsSuccessAlertAndNewGridRow()
    {
        var name = NewName();

        await CreateAsync(name);

        await DeleteAsync(name);
    }

    [Test]
    public async Task Update_ChangesNameInGrid()
    {
        var name = NewName();
        var updatedName = NewName();

        await CreateAsync(name);
        await UpdateAsync(name, updatedName);

        await DeleteAsync(updatedName);
    }

    [Test]
    public async Task Delete_RemovesRowFromGrid()
    {
        var name = NewName();

        await CreateAsync(name);
        await DeleteAsync(name);
    }
}
