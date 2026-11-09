using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DashboardManager.Queries;


public class GetPurchaseDashboardDto
{
    public List<PurchaseOrderItem>? PurchaseOrderDashboard { get; init; }
    public List<BarSeries>? PurchaseByVendorGroupDashboard { get; init; }
    public List<BarSeries>? PurchaseByVendorCategoryDashboard { get; init; }
}

public class GetPurchaseDashboardResult
{
    public GetPurchaseDashboardDto? Data { get; init; }
}

public class GetPurchaseDashboardRequest : IRequest<GetPurchaseDashboardResult>
{
}

public class GetPurchaseDashboardHandler : IRequestHandler<GetPurchaseDashboardRequest, GetPurchaseDashboardResult>
{
    private readonly IQueryContext _context;

    public GetPurchaseDashboardHandler(IQueryContext context)
    {
        _context = context;
    }

    public async Task<GetPurchaseDashboardResult> Handle(GetPurchaseDashboardRequest request, CancellationToken cancellationToken)
    {

        var purchaseOrderItemData = await _context.PurchaseOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.PurchaseOrder)
            .Include(x => x.Product)
            .Where(x => x.PurchaseOrder!.OrderStatus == PurchaseOrderStatus.Confirmed)
            .OrderByDescending(x => x.PurchaseOrder!.OrderDate)
            .Take(30)
            .ToListAsync(cancellationToken);

        var purchaseByVendorGroupData = _context.PurchaseOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
                .Include(x => x.PurchaseOrder)
                    .ThenInclude(x => x!.Vendor)
                        .ThenInclude(x => x!.VendorGroup)
                .Include(x => x.Product)
                .Where(x => x.Product!.Physical == true)
            .Select(x => new
            {
                Status = x.PurchaseOrder!.OrderStatus,
                VendorGroupName = x.PurchaseOrder!.Vendor!.VendorGroup!.Name,
                Quantity = x.Quantity
            })
            .GroupBy(x => new { x.Status, x.VendorGroupName })
            .Select(g => new
            {
                Status = g.Key.Status,
                VendorGroupName = g.Key.VendorGroupName,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();

        var purchaseByVendorCategoryDate = _context.PurchaseOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.PurchaseOrder)
                .ThenInclude(x => x!.Vendor)
                    .ThenInclude(x => x!.VendorCategory)
            .Include(x => x.Product)
            .Where(x => x.Product!.Physical == true)
            .Select(x => new
            {
                Status = x.PurchaseOrder!.OrderStatus,
                VendorCategoryName = x.PurchaseOrder!.Vendor!.VendorCategory!.Name,
                Quantity = x.Quantity
            })
            .GroupBy(x => new { x.Status, x.VendorCategoryName })
            .Select(g => new
            {
                Status = g.Key.Status,
                VendorCategoryName = g.Key.VendorCategoryName,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();


        var result = new GetPurchaseDashboardResult
        {
            Data = new GetPurchaseDashboardDto
            {
                PurchaseOrderDashboard = purchaseOrderItemData,
                PurchaseByVendorGroupDashboard =
                    Enum.GetValues(typeof(PurchaseOrderStatus))
                    .Cast<PurchaseOrderStatus>()
                    .Select(status => new BarSeries
                    {
                        Type = "Bar",
                        XName = "x",
                        Width = 2,
                        YName = "y",
                        Name = Enum.GetName(typeof(PurchaseOrderStatus), status)!,
                        ColumnSpacing = 0.1,
                        TooltipMappingName = "tooltipMappingName",
                        DataSource = purchaseByVendorGroupData
                            .Where(x => x.Status == status)
                            .Select(x => new BarDataItem
                            {
                                X = x.VendorGroupName ?? "",
                                TooltipMappingName = x.VendorGroupName ?? "",
                                Y = (int)x.Quantity!.Value
                            }).ToList()
                    })
                    .ToList(),
                PurchaseByVendorCategoryDashboard =
                    Enum.GetValues(typeof(PurchaseOrderStatus))
                    .Cast<PurchaseOrderStatus>()
                    .Select(status => new BarSeries
                    {
                        Type = "Column",
                        XName = "x",
                        Width = 2,
                        YName = "y",
                        Name = Enum.GetName(typeof(PurchaseOrderStatus), status)!,
                        ColumnSpacing = 0.1,
                        TooltipMappingName = "tooltipMappingName",
                        DataSource = purchaseByVendorCategoryDate
                            .Where(x => x.Status == status)
                            .Select(x => new BarDataItem
                            {
                                X = x.VendorCategoryName ?? "",
                                TooltipMappingName = x.VendorCategoryName ?? "",
                                Y = (int)x.Quantity!.Value
                            }).ToList()
                    })
                    .ToList(),
            }
        };

        return result;
    }
}
