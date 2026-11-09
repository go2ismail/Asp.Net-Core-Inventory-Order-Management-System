using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DashboardManager.Queries;


public class GetSalesDashboardDto
{
    public List<SalesOrderItem>? SalesOrderDashboard { get; init; }
    public List<BarSeries>? SalesByCustomerGroupDashboard { get; init; }
    public List<BarSeries>? SalesByCustomerCategoryDashboard { get; init; }
}

public class GetSalesDashboardResult
{
    public GetSalesDashboardDto? Data { get; init; }
}

public class GetSalesDashboardRequest : IRequest<GetSalesDashboardResult>
{
}

public class GetSalesDashboardHandler : IRequestHandler<GetSalesDashboardRequest, GetSalesDashboardResult>
{
    private readonly IQueryContext _context;

    public GetSalesDashboardHandler(IQueryContext context)
    {
        _context = context;
    }

    public async Task<GetSalesDashboardResult> Handle(GetSalesDashboardRequest request, CancellationToken cancellationToken)
    {

        var salesOrderItemData = await _context.SalesOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.SalesOrder)
            .Include(x => x.Product)
            .Where(x => x.SalesOrder!.OrderStatus == SalesOrderStatus.Confirmed)
            .OrderByDescending(x => x.SalesOrder!.OrderDate)
            .Take(30)
            .ToListAsync(cancellationToken);

        var salesByCustomerGroupData = _context.SalesOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
                .Include(x => x.SalesOrder)
                    .ThenInclude(x => x!.Customer)
                        .ThenInclude(x => x!.CustomerGroup)
                .Include(x => x.Product)
                .Where(x => x.Product!.Physical == true)
            .Select(x => new
            {
                Status = x.SalesOrder!.OrderStatus,
                CustomerGroupName = x.SalesOrder!.Customer!.CustomerGroup!.Name,
                Quantity = x.Quantity
            })
            .GroupBy(x => new { x.Status, x.CustomerGroupName })
            .Select(g => new
            {
                Status = g.Key.Status,
                CustomerGroupName = g.Key.CustomerGroupName,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();

        var salesByCustomerCategoryData = _context.SalesOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.SalesOrder)
                .ThenInclude(x => x!.Customer)
                    .ThenInclude(x => x!.CustomerCategory)
            .Include(x => x.Product)
            .Where(x => x.Product!.Physical == true)
            .Select(x => new
            {
                Status = x.SalesOrder!.OrderStatus,
                CustomerCategoryName = x.SalesOrder!.Customer!.CustomerCategory!.Name,
                Quantity = x.Quantity
            })
            .GroupBy(x => new { x.Status, x.CustomerCategoryName })
            .Select(g => new
            {
                Status = g.Key.Status,
                CustomerCategoryName = g.Key.CustomerCategoryName,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();


        var result = new GetSalesDashboardResult
        {
            Data = new GetSalesDashboardDto
            {
                SalesOrderDashboard = salesOrderItemData,
                SalesByCustomerGroupDashboard =
                    Enum.GetValues(typeof(SalesOrderStatus))
                    .Cast<SalesOrderStatus>()
                    .Select(status => new BarSeries
                    {
                        Type = "Column",
                        XName = "x",
                        Width = 2,
                        YName = "y",
                        Name = Enum.GetName(typeof(SalesOrderStatus), status)!,
                        ColumnSpacing = 0.1,
                        TooltipMappingName = "tooltipMappingName",
                        DataSource = salesByCustomerGroupData
                            .Where(x => x.Status == status)
                            .Select(x => new BarDataItem
                            {
                                X = x.CustomerGroupName ?? "",
                                TooltipMappingName = x.CustomerGroupName ?? "",
                                Y = (int)x.Quantity!.Value
                            }).ToList()
                    })
                    .ToList(),
                SalesByCustomerCategoryDashboard =
                    Enum.GetValues(typeof(SalesOrderStatus))
                    .Cast<SalesOrderStatus>()
                    .Select(status => new BarSeries
                    {
                        Type = "Bar",
                        XName = "x",
                        Width = 2,
                        YName = "y",
                        Name = Enum.GetName(typeof(SalesOrderStatus), status)!,
                        ColumnSpacing = 0.1,
                        TooltipMappingName = "tooltipMappingName",
                        DataSource = salesByCustomerCategoryData
                            .Where(x => x.Status == status)
                            .Select(x => new BarDataItem
                            {
                                X = x.CustomerCategoryName ?? "",
                                TooltipMappingName = x.CustomerCategoryName ?? "",
                                Y = (int)x.Quantity!.Value
                            }).ToList()
                    })
                    .ToList()
            }
        };

        return result;
    }
}
