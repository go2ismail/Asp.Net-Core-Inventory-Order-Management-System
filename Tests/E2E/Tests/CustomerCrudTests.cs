using NUnit.Framework;

namespace E2E;

[TestFixture]
public class CustomerCrudTests : CrudModuleTestBase
{
    // Seeded by Infrastructure/SeedManager/Demos/CustomerGroupSeeder.cs and CustomerCategorySeeder.cs.
    private const string GroupName = "Corporate";
    private const string CategoryName = "Enterprise";

    protected override string ListPath => "/Customers/CustomerList";

    protected override string ListTitle => "Customer List";

    protected override string NamePrefix => "E2E-CUS";

    [SetUp]
    public async Task SignInAsync()
    {
        await LoginAsync();
        await OpenListAsync();
    }

    protected override async Task FillFormAsync(string name)
    {
        await FillTextBoxAsync("Name", name);
        await SelectDropDownAsync("CustomerGroupId", GroupName);
        await SelectDropDownAsync("CustomerCategoryId", CategoryName);
        await FillTextBoxAsync("Description", $"E2E customer {name}");
        await FillTextBoxAsync("Street", "Jl. E2E No. 1");
        await FillTextBoxAsync("City", "Bandung");
        await FillTextBoxAsync("State", "Jawa Barat");
        await FillTextBoxAsync("ZipCode", "40111");
        await FillTextBoxAsync("Country", "Indonesia");
        await FillTextBoxAsync("PhoneNumber", "+62 22 1234567");
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
