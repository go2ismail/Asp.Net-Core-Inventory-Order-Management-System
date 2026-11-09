using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DashboardManager.Queries;


public class GetCardsDashboardDto
{
    public CardsItem? CardsDashboard { get; init; }
}

public class GetCardsDashboardResult
{
    public GetCardsDashboardDto? Data { get; init; }
}

public class GetCardsDashboardRequest : IRequest<GetCardsDashboardResult>
{
}

public class GetCardsDashboardHandler : IRequestHandler<GetCardsDashboardRequest, GetCardsDashboardResult>
{
    private readonly IQueryContext _context;

    public GetCardsDashboardHandler(IQueryContext context)
    {
        _context = context;
    }

    public async Task<GetCardsDashboardResult> Handle(GetCardsDashboardRequest request, CancellationToken cancellationToken)
    {
        var salesTotal = await _context.SalesOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .SumAsync(x => (double?)x.Quantity, cancellationToken);

        var salesReturnTotal = await _context.InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.ModuleName == nameof(SalesReturn) && x.Status == InventoryTransactionStatus.Confirmed && x.Warehouse!.SystemWarehouse == false)
            .SumAsync(x => (double?)x.Movement, cancellationToken);

        var purchaseTotal = await _context.PurchaseOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .SumAsync(x => (double?)x.Quantity, cancellationToken);

        var purchaseReturnTotal = await _context.InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.ModuleName == nameof(PurchaseReturn) && x.Status == InventoryTransactionStatus.Confirmed && x.Warehouse!.SystemWarehouse == false)
            .SumAsync(x => (double?)x.Movement, cancellationToken);

        var deliveryOrderTotal = await _context.InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.ModuleName == nameof(DeliveryOrder) && x.Status == InventoryTransactionStatus.Confirmed && x.Warehouse!.SystemWarehouse == false)
            .SumAsync(x => (double?)x.Movement, cancellationToken);

        var goodsReceiveTotal = await _context.InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.ModuleName == nameof(GoodsReceive) && x.Status == InventoryTransactionStatus.Confirmed && x.Warehouse!.SystemWarehouse == false)
            .SumAsync(x => (double?)x.Movement, cancellationToken);

        var transferOutTotal = await _context.InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.ModuleName == nameof(TransferOut) && x.Status == InventoryTransactionStatus.Confirmed && x.Warehouse!.SystemWarehouse == false)
            .SumAsync(x => (double?)x.Movement, cancellationToken);

        var transferInTotal = await _context.InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.ModuleName == nameof(TransferIn) && x.Status == InventoryTransactionStatus.Confirmed && x.Warehouse!.SystemWarehouse == false)
            .SumAsync(x => (double?)x.Movement, cancellationToken);

        var cardsDashboardData = new CardsItem
        {
            SalesTotal = salesTotal,
            SalesReturnTotal = salesReturnTotal,
            PurchaseTotal = purchaseTotal,
            PurchaseReturnTotal = purchaseReturnTotal,
            DeliveryOrderTotal = deliveryOrderTotal,
            GoodsReceiveTotal = goodsReceiveTotal,
            TransferOutTotal = transferOutTotal,
            TransferInTotal = transferInTotal
        };



        var result = new GetCardsDashboardResult
        {
            Data = new GetCardsDashboardDto
            {
                CardsDashboard = cardsDashboardData
            }
        };

        return result;
    }
}
