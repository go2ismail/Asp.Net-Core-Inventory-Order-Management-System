using Application.Common.Services.SecurityManager;
using System.Text.Json;

namespace Infrastructure.SecurityManager.NavigationMenu;






public class JsonStructureItem
{
    public string? URL { get; set; }
    public string? Name { get; set; }
    public bool IsModule { get; set; }
    public List<JsonStructureItem> Children { get; set; } = new List<JsonStructureItem>();
}

public static class NavigationTreeStructure
{

    public static readonly string JsonStructure = """
    [
        {
            "URL": "#",
            "Name": "Dashboards",
            "IsModule": true,
            "Children": [
                {
                    "URL": "/Dashboards/DefaultDashboard",
                    "Name": "Default",
                    "IsModule": false
                }
            ]
        },
        {
            "URL": "#",
            "Name": "Sales",
            "IsModule": true,
            "Children": [
                {
                    "URL": "/CustomerGroups/CustomerGroupList",
                    "Name": "Customer Group",
                    "IsModule": false
                },
                {
                    "URL": "/CustomerCategories/CustomerCategoryList",
                    "Name": "Customer Category",
                    "IsModule": false
                },
                {
                    "URL": "/Customers/CustomerList",
                    "Name": "Customer",
                    "IsModule": false
                },
                {
                    "URL": "/CustomerContacts/CustomerContactList",
                    "Name": "Customer Contact",
                    "IsModule": false
                },
                {
                    "URL": "/SalesOrders/SalesOrderList",
                    "Name": "Sales Order",
                    "IsModule": false
                },
                {
                    "URL": "/SalesReports/SalesReportList",
                    "Name": "Sales Report",
                    "IsModule": false
                }
            ]
        },
        {
            "URL": "#",
            "Name": "Purchase",
            "IsModule": true,
            "Children": [
                {
                    "URL": "/VendorGroups/VendorGroupList",
                    "Name": "Vendor Group",
                    "IsModule": false
                },
                {
                    "URL": "/VendorCategories/VendorCategoryList",
                    "Name": "Vendor Category",
                    "IsModule": false
                },
                {
                    "URL": "/Vendors/VendorList",
                    "Name": "Vendor",
                    "IsModule": false
                },
                {
                    "URL": "/VendorContacts/VendorContactList",
                    "Name": "Vendor Contact",
                    "IsModule": false
                },
                {
                    "URL": "/PurchaseOrders/PurchaseOrderList",
                    "Name": "Purchase Order",
                    "IsModule": false
                },
                {
                    "URL": "/PurchaseReports/PurchaseReportList",
                    "Name": "Purchase Report",
                    "IsModule": false
                }
            ]
        },
        {
            "URL": "#",
            "Name": "Inventory",
            "IsModule": true,
            "Children": [
                {
                    "URL": "/UnitMeasures/UnitMeasureList",
                    "Name": "Unit Measure",
                    "IsModule": false
                },
                {
                    "URL": "/ProductGroups/ProductGroupList",
                    "Name": "Product Group",
                    "IsModule": false
                },
                {
                    "URL": "/Products/ProductList",
                    "Name": "Product",
                    "IsModule": false
                },
                {
                    "URL": "/Warehouses/WarehouseList",
                    "Name": "Warehouse",
                    "IsModule": false
                },
                {
                    "URL": "/DeliveryOrders/DeliveryOrderList",
                    "Name": "Delivery Order",
                    "IsModule": false
                },
                {
                    "URL": "/SalesReturns/SalesReturnList",
                    "Name": "Sales Return",
                    "IsModule": false
                },
                {
                    "URL": "/GoodsReceives/GoodsReceiveList",
                    "Name": "Goods Receive",
                    "IsModule": false
                },
                {
                    "URL": "/PurchaseReturns/PurchaseReturnList",
                    "Name": "Purchase Return",
                    "IsModule": false
                },
                {
                    "URL": "/TransferOuts/TransferOutList",
                    "Name": "Transfer Out",
                    "IsModule": false
                },
                {
                    "URL": "/TransferIns/TransferInList",
                    "Name": "Transfer In",
                    "IsModule": false
                },
                {
                    "URL": "/PositiveAdjustments/PositiveAdjustmentList",
                    "Name": "Positive Adjustment",
                    "IsModule": false
                },
                {
                    "URL": "/NegativeAdjustments/NegativeAdjustmentList",
                    "Name": "Negative Adjustment",
                    "IsModule": false
                },
                {
                    "URL": "/Scrappings/ScrappingList",
                    "Name": "Scrapping",
                    "IsModule": false
                },
                {
                    "URL": "/StockCounts/StockCountList",
                    "Name": "Stock Count",
                    "IsModule": false
                },
                {
                    "URL": "/TransactionReports/TransactionReportList",
                    "Name": "Transaction Report",
                    "IsModule": false
                },
                {
                    "URL": "/StockReports/StockReportList",
                    "Name": "Stock Report",
                    "IsModule": false
                },
                {
                    "URL": "/MovementReports/MovementReportList",
                    "Name": "Movement Reports",
                    "IsModule": false
                }
            ]
        },
        {
            "URL": "#",
            "Name": "Utilities",
            "IsModule": true,
            "Children": [   
                {
                    "URL": "/Todos/TodoList",
                    "Name": "Todo",
                    "IsModule": false
                },
                {
                    "URL": "/TodoItems/TodoItemList",
                    "Name": "Todo Item",
                    "IsModule": false
                }
            ]
        },
        {
            "URL": "#",
            "Name": "Membership",
            "IsModule": true,
            "Children": [
                {
                    "URL": "/Users/UserList",
                    "Name": "Users",
                    "IsModule": false
                },
                {
                    "URL": "/Roles/RoleList",
                    "Name": "Roles",
                    "IsModule": false
                }
            ]
        },
        {
            "URL": "#",
            "Name": "Profiles",
            "IsModule": true,
            "Children": [
                {
                    "URL": "/Profiles/MyProfile",
                    "Name": "My Profile",
                    "IsModule": false
                }
            ]
        },
        {
            "URL": "#",
            "Name": "Settings",
            "IsModule": true,
            "Children": [
                {
                    "URL": "/Companies/MyCompany",
                    "Name": "My Company",
                    "IsModule": false
                },
                {
                    "URL": "/Taxs/TaxList",
                    "Name": "Tax",
                    "IsModule": false
                },
                {
                    "URL": "/NumberSequences/NumberSequenceList",
                    "Name": "Number Sequence",
                    "IsModule": false
                }
            ]
        }
    ]
    """;

    public static List<MenuNavigationTreeNodeDto> GetCompleteMenuNavigationTreeNode()
    {
        var json = JsonStructure;

        var menus = JsonSerializer.Deserialize<List<JsonStructureItem>>(json);

        List<MenuNavigationTreeNodeDto> nodes = new List<MenuNavigationTreeNodeDto>();

        var index = 1;
        void AddNodes(List<JsonStructureItem> menuItems, string? parentId = null)
        {
            foreach (var item in menuItems)
            {
                var nodeId = index.ToString();
                if (item.IsModule)
                {
                    nodes.Add(new MenuNavigationTreeNodeDto(nodeId, item.Name ?? "", param_hasChild: true, param_expanded: false));
                }
                else
                {
                    nodes.Add(new MenuNavigationTreeNodeDto(nodeId, item.Name ?? "", parentId, item.URL));
                }

                index++;

                if (item.Children != null && item.Children.Count > 0)
                {
                    AddNodes(item.Children, nodeId);
                }
            }
        }

        if (menus != null) AddNodes(menus);

        return nodes;
    }

    public static string GetFirstSegmentFromUrlPath(string? path)
    {
        var result = string.Empty;
        if (path != null && path.Contains("/"))
        {
            string[] parts = path.Split("/");
            if (parts.Length > 2)
            {
                result = parts[1];
            }
        }
        return result;
    }

    public static List<string> GetCompleteFirstMenuNavigationSegment()
    {
        var json = JsonStructure;
        var menus = JsonSerializer.Deserialize<List<JsonStructureItem>>(json);
        var result = new List<string>();

        if (menus != null)
        {
            foreach (var item in menus)
            {
                ProcessMenuItem(item, result);
            }
        }

        return result;
    }

    private static void ProcessMenuItem(JsonStructureItem item, List<string> result)
    {
        if (!string.IsNullOrEmpty(item.URL) && item.URL != "#")
        {
            var segment = GetFirstSegmentFromUrlPath(item.URL);
            if (!string.IsNullOrEmpty(segment) && !result.Contains(segment))
            {
                result.Add(segment);
            }
        }

        if (item.Children != null)
        {
            foreach (var child in item.Children)
            {
                ProcessMenuItem(child, result);
            }
        }
    }


}

