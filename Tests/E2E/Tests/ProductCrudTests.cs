using NUnit.Framework;

namespace E2E;

[TestFixture]
public class ProductCrudTests : CrudModuleTestBase
{
    // Seeded by Infrastructure/SeedManager/Demos/ProductGroupSeeder.cs and UnitMeasureSeeder.cs.
    private const string GroupName = "Hardware";
    private const string UnitMeasureName = "pcs";

    protected override string ListPath => "/Products/ProductList";

    protected override string ListTitle => "Product List";

    protected override string NamePrefix => "E2E-PRD";

    [SetUp]
    public async Task SignInAsync()
    {
        await LoginAsync();
        await OpenListAsync();
    }

    protected override async Task FillFormAsync(string name)
    {
        await FillTextBoxAsync("Name", name);
        await FillTextBoxAsync("UnitPrice", "19.99");
        await SelectDropDownAsync("UnitMeasureId", UnitMeasureName);
        await SelectDropDownAsync("ProductGroupId", GroupName);
        await FillTextBoxAsync("Description", $"E2E product {name}");
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
