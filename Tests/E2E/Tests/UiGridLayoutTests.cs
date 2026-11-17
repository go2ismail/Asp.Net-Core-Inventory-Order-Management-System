using System.Text.Json;
using Microsoft.Playwright;
using NUnit.Framework;

namespace E2E;

[TestFixture]
public class UiGridLayoutTests : E2ETestBase
{
    private sealed record ListRoute(string Name, string Url, string ControlSelector);

    private sealed record Rect(double Top, double Right, double Bottom, double Left);

    private sealed record Padding(string Top, string Right, string Bottom, string Left);

    private sealed record ControlRect(double Top, double Right, double Bottom, double Left, double Height);

    private sealed record LayoutMetrics(
        Padding Padding,
        Rect Wrapper,
        ControlRect Control,
        double FooterTop,
        double ScrollHeight,
        double ClientHeight,
        double ScrollWidth,
        double ClientWidth);

    private sealed record GridHeight(string? Height, bool InManageContactModal);

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private static readonly ListRoute CustomerList = new("Customer", "/Customers/CustomerList", ".e-grid");

    private static readonly ListRoute RoleList = new("Roles", "/Roles/RoleList", ".e-grid");

    private static readonly ListRoute[] ListRoutes =
    {
        new("Customer Group", "/CustomerGroups/CustomerGroupList", ".e-grid"),
        new("Customer Category", "/CustomerCategories/CustomerCategoryList", ".e-grid"),
        CustomerList,
        new("Customer Contact", "/CustomerContacts/CustomerContactList", ".e-grid"),
        new("Sales Order", "/SalesOrders/SalesOrderList", ".e-grid"),
        new("Sales Report", "/SalesReports/SalesReportList", ".e-grid"),
        new("Vendor Group", "/VendorGroups/VendorGroupList", ".e-grid"),
        new("Vendor Category", "/VendorCategories/VendorCategoryList", ".e-grid"),
        new("Vendor", "/Vendors/VendorList", ".e-grid"),
        new("Vendor Contact", "/VendorContacts/VendorContactList", ".e-grid"),
        new("Purchase Order", "/PurchaseOrders/PurchaseOrderList", ".e-grid"),
        new("Purchase Report", "/PurchaseReports/PurchaseReportList", ".e-grid"),
        new("Unit Measure", "/UnitMeasures/UnitMeasureList", ".e-grid"),
        new("Product Group", "/ProductGroups/ProductGroupList", ".e-grid"),
        new("Product", "/Products/ProductList", ".e-grid"),
        new("Warehouse", "/Warehouses/WarehouseList", ".e-grid"),
        new("Delivery Order", "/DeliveryOrders/DeliveryOrderList", ".e-grid"),
        new("Sales Return", "/SalesReturns/SalesReturnList", ".e-grid"),
        new("Goods Receive", "/GoodsReceives/GoodsReceiveList", ".e-grid"),
        new("Purchase Return", "/PurchaseReturns/PurchaseReturnList", ".e-grid"),
        new("Transfer Out", "/TransferOuts/TransferOutList", ".e-grid"),
        new("Transfer In", "/TransferIns/TransferInList", ".e-grid"),
        new("Positive Adjustment", "/PositiveAdjustments/PositiveAdjustmentList", ".e-grid"),
        new("Negative Adjustment", "/NegativeAdjustments/NegativeAdjustmentList", ".e-grid"),
        new("Scrapping", "/Scrappings/ScrappingList", ".e-grid"),
        new("Stock Count", "/StockCounts/StockCountList", ".e-grid"),
        new("Transaction Report", "/TransactionReports/TransactionReportList", ".e-grid"),
        new("Stock Report", "/StockReports/StockReportList", ".e-grid"),
        new("Movement Reports", "/MovementReports/MovementReportList", ".e-pivotview"),
        new("Todo", "/Todos/TodoList", ".e-grid"),
        new("Todo Item", "/TodoItems/TodoItemList", ".e-grid"),
        new("Users", "/Users/UserList", ".e-grid"),
        RoleList,
        new("Tax", "/Taxs/TaxList", ".e-grid"),
        new("Number Sequence", "/NumberSequences/NumberSequenceList", ".e-grid"),
    };

    [Test]
    public async Task EveryListPage_RendersAFullBleedControlAboveTheFooter()
    {
        Assert.That(ListRoutes, Has.Length.EqualTo(35), "All list pages should be covered.");

        await LoginAsync();

        foreach (var route in ListRoutes)
        {
            await OpenListAsync(route);
            var metrics = await MeasureLayoutAsync(route.ControlSelector);
            AssertFullBleed(metrics, route.Name);
        }
    }

    [Test]
    public async Task Pager_SitsAtTheBottomOfAPopulatedGrid()
    {
        await LoginAsync();
        await OpenListAsync(CustomerList);

        await Page.EvaluateAsync(@"() => {
            const element = document.querySelector('.e-grid');
            const grid = element && element.ej2_instances && element.ej2_instances[0];
            if (grid && grid.pageSettings) {
                grid.pageSettings.pageSize = 1000;
                if (typeof grid.refresh === 'function') {
                    grid.refresh();
                }
            }
        }");

        var pager = Page.Locator(".e-grid .e-gridpager").First;
        await Expect(pager).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 20000 });
        await Page.WaitForTimeoutAsync(300);

        var metrics = await Page.EvaluateAsync<JsonElement>(@"() => {
            const grid = document.querySelector('.e-grid');
            const pager = document.querySelector('.e-grid .e-gridpager');
            const gridRect = grid.getBoundingClientRect();
            const pagerRect = pager.getBoundingClientRect();
            return {
                gridHeight: gridRect.height,
                gridBottom: gridRect.bottom,
                pagerBottom: pagerRect.bottom,
            };
        }");

        var gridHeight = metrics.GetProperty("gridHeight").GetDouble();
        var gridBottom = metrics.GetProperty("gridBottom").GetDouble();
        var pagerBottom = metrics.GetProperty("pagerBottom").GetDouble();

        Assert.Multiple(() =>
        {
            Assert.That(gridHeight, Is.GreaterThan(400), "Customer: grid height should fill the viewport");
            Assert.That(
                Math.Abs(gridBottom - pagerBottom),
                Is.LessThanOrEqualTo(4),
                "Customer: pager bottom should sit at the grid bottom");
        });
    }

    [Test]
    public async Task ModalSubGrids_KeepTheirConfiguredHeight()
    {
        await LoginAsync();
        await OpenListAsync(CustomerList);

        var heightsJson = await Page.EvaluateAsync<JsonElement>(@"() => Array.from(document.querySelectorAll('.e-grid')).map((element) => {
            const grid = element.ej2_instances && element.ej2_instances[0];
            return {
                height: grid ? grid.height : null,
                inManageContactModal: Boolean(element.closest('#ManageContactModal')),
            };
        })");

        var heights = heightsJson.Deserialize<List<GridHeight>>(JsonOptions) ?? new List<GridHeight>();
        var mainGrid = heights.FirstOrDefault(item => !item.InManageContactModal);
        var modalGrid = heights.FirstOrDefault(item => item.InManageContactModal);

        Assert.Multiple(() =>
        {
            Assert.That(mainGrid, Is.Not.Null, "Customer: main grid should exist");
            Assert.That(mainGrid?.Height, Is.EqualTo("100%"), "Customer: main grid should fill its container");
            Assert.That(modalGrid, Is.Not.Null, "Customer: Manage Contact modal grid should exist");
            Assert.That(modalGrid?.Height, Is.EqualTo("240px"), "Customer: Manage Contact modal grid should keep its fixed height");
        });

        await Page.Locator("#AddCustom").ClickAsync();
        await Expect(Page.Locator("#MainModal")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 20000 });
        await Page.Locator("#MainModal .modal-footer button[data-bs-dismiss=\"modal\"]").ClickAsync();
        await Expect(Page.Locator("#MainModal")).ToBeHiddenAsync(new LocatorAssertionsToBeHiddenOptions { Timeout = 20000 });
    }

    [Test]
    public async Task FullBleedLayout_StaysFlushAfterAViewportResize()
    {
        await LoginAsync();

        await Page.SetViewportSizeAsync(1280, 720);
        await OpenListAsync(RoleList);
        AssertFullBleed(await MeasureLayoutAsync(RoleList.ControlSelector), "1280x720");

        await Page.SetViewportSizeAsync(1600, 900);
        await Page.WaitForFunctionAsync(
            @"() => {
                const grid = document.querySelector('.e-grid');
                return grid !== null && grid.getBoundingClientRect().height > 400;
            }",
            null,
            new PageWaitForFunctionOptions { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(500);

        var resized = await MeasureLayoutAsync(RoleList.ControlSelector);

        Assert.Multiple(() =>
        {
            Assert.That(resized.ClientWidth, Is.EqualTo(1600), "1600x900: document client width");
            Assert.That(resized.ClientHeight, Is.EqualTo(900), "1600x900: document client height");
        });

        AssertFullBleed(resized, "1600x900");
    }

    private async Task OpenListAsync(ListRoute route)
    {
        await Page.GotoAsync($"{BaseUrl}{route.Url}", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        await Page.WaitForFunctionAsync(
            @"() => {
                const app = document.querySelector('#app');
                return app !== null && !app.hasAttribute('v-cloak');
            }",
            null,
            new PageWaitForFunctionOptions { Timeout = 60000 });

        await Expect(Page.Locator(route.ControlSelector).First).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 60000
        });

        await Page.WaitForFunctionAsync(
            @"(selector) => {
                const control = document.querySelector(selector);
                return control !== null && control.getBoundingClientRect().height > 400;
            }",
            route.ControlSelector,
            new PageWaitForFunctionOptions { Timeout = 30000 });

        await Page.WaitForTimeoutAsync(200);
    }

    private async Task<LayoutMetrics> MeasureLayoutAsync(string controlSelector)
    {
        var json = await Page.EvaluateAsync<JsonElement>(@"(selector) => {
            const wrapper = document.querySelector('.content-wrapper');
            const control = document.querySelector(selector);
            const footer = document.querySelector('.main-footer');
            const style = getComputedStyle(wrapper);
            const wrapperRect = wrapper.getBoundingClientRect();
            const controlRect = control.getBoundingClientRect();
            const footerRect = footer.getBoundingClientRect();
            const doc = document.documentElement;
            const toRect = (rect) => ({ top: rect.top, right: rect.right, bottom: rect.bottom, left: rect.left });

            return {
                padding: {
                    top: style.paddingTop,
                    right: style.paddingRight,
                    bottom: style.paddingBottom,
                    left: style.paddingLeft,
                },
                wrapper: toRect(wrapperRect),
                control: { ...toRect(controlRect), height: controlRect.height },
                footerTop: footerRect.top,
                scrollHeight: doc.scrollHeight,
                clientHeight: doc.clientHeight,
                scrollWidth: doc.scrollWidth,
                clientWidth: doc.clientWidth,
            };
        }", controlSelector);

        return json.Deserialize<LayoutMetrics>(JsonOptions)
            ?? throw new InvalidOperationException("Could not read layout metrics from the page.");
    }

    private static void AssertFullBleed(LayoutMetrics metrics, string label)
    {
        Assert.Multiple(() =>
        {
            Assert.That(metrics.Padding.Top, Is.EqualTo("0px"), $"{label}: content wrapper top padding");
            Assert.That(metrics.Padding.Right, Is.EqualTo("0px"), $"{label}: content wrapper right padding");
            Assert.That(metrics.Padding.Bottom, Is.EqualTo("0px"), $"{label}: content wrapper bottom padding");
            Assert.That(metrics.Padding.Left, Is.EqualTo("0px"), $"{label}: content wrapper left padding");
            Assert.That(
                Math.Abs(metrics.Control.Top - metrics.Wrapper.Top),
                Is.LessThanOrEqualTo(2),
                $"{label}: control top should be flush with the content wrapper top");
            Assert.That(
                Math.Abs(metrics.Control.Left - metrics.Wrapper.Left),
                Is.LessThanOrEqualTo(2),
                $"{label}: control left should be flush with the content wrapper left");
            Assert.That(
                Math.Abs(metrics.Control.Right - metrics.Wrapper.Right),
                Is.LessThanOrEqualTo(2),
                $"{label}: control right should be flush with the content wrapper right");
            Assert.That(
                Math.Abs(metrics.Control.Bottom - metrics.Wrapper.Bottom),
                Is.LessThanOrEqualTo(2),
                $"{label}: control bottom should be flush with the content wrapper bottom");
            Assert.That(
                Math.Abs(metrics.Wrapper.Bottom - metrics.FooterTop),
                Is.LessThanOrEqualTo(2),
                $"{label}: content wrapper bottom should touch the footer top");
            Assert.That(metrics.Control.Height, Is.GreaterThan(400), $"{label}: control height should fill the viewport");
            Assert.That(
                metrics.ScrollHeight,
                Is.LessThanOrEqualTo(metrics.ClientHeight + 1),
                $"{label}: the page should not scroll vertically");
            Assert.That(
                metrics.ScrollWidth,
                Is.LessThanOrEqualTo(metrics.ClientWidth + 1),
                $"{label}: the page should not scroll horizontally");
        });
    }
}
