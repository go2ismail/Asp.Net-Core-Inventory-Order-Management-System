using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DashboardManager.Queries;


public class GetInventoryDashboardDto
{
    public List<InventoryTransaction>? InventoryTransactionDashboard { get; init; }
    public List<BarSeries>? InventoryStockDashboard { get; init; }
}

public class GetInventoryDashboardResult
{
    public GetInventoryDashboardDto? Data { get; init; }
}

public class GetInventoryDashboardRequest : IRequest<GetInventoryDashboardResult>
{
}

public class GetInventoryDashboardHandler : IRequestHandler<GetInventoryDashboardRequest, GetInventoryDashboardResult>
{
    private readonly IQueryContext _context;

    public GetInventoryDashboardHandler(IQueryContext context)
    {
        _context = context;
    }

    public async Task<GetInventoryDashboardResult> Handle(GetInventoryDashboardRequest request, CancellationToken cancellationToken)
    {

        var inventoryTransactionData = await _context.InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.Warehouse)
            .Include(x => x.Product)
            .Include(x => x.WarehouseFrom)
            .Include(x => x.WarehouseTo)
            .Where(x =>
                x.Product!.Physical == true &&
                x.Warehouse!.SystemWarehouse == false &&
                x.Status == InventoryTransactionStatus.Confirmed
            )
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);


        var inventoryStockData = _context.InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.Warehouse)
            .Include(x => x.Product)
            .Where(x =>
                x.Status == InventoryTransactionStatus.Confirmed &&
                x.Warehouse!.SystemWarehouse == false &&
                x.Product!.Physical == true
            )
            .GroupBy(x => new { x.WarehouseId, x.ProductId })
            .Select(group => new
            {
                WarehouseId = group.Key.WarehouseId,
                ProductId = group.Key.ProductId,
                Warehouse = group.Max(x => x.Warehouse!.Name),
                Product = group.Max(x => x.Product!.Name),
                Stock = group.Sum(x => x.Stock),
                Id = group.Max(x => x.Id),
                CreatedAtUtc = group.Max(x => x.CreatedAtUtc)
            })
        .ToList();

        var warehouseData = _context.Warehouse
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.SystemWarehouse == false)
            .Select(x => x.Name)
            .ToList();


        var result = new GetInventoryDashboardResult
        {
            Data = new GetInventoryDashboardDto
            {
                InventoryTransactionDashboard = inventoryTransactionData,
                InventoryStockDashboard =
                    warehouseData
                    .Select(wh => new BarSeries
                    {
                        Type = "Column",
                        XName = "x",
                        Width = 2,
                        YName = "y",
                        Name = wh ?? "",
                        ColumnSpacing = 0.1,
                        TooltipMappingName = "tooltipMappingName",
                        DataSource = inventoryStockData
                            .Where(x => x.Warehouse == wh)
                            .Select(x => new BarDataItem
                            {
                                X = x.Product ?? string.Empty,
                                TooltipMappingName = x.Product ?? string.Empty,
                                Y = (int)(x.Stock ?? 0.0)
                            }).ToList()
                    })
                    .ToList()
            }
        };

        return result;
    }
}
