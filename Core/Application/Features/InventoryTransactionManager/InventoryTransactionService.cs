using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Application.Features.WarehouseManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.InventoryTransactionManager;

public partial class InventoryTransactionService
{
    private readonly NumberSequenceService _numberSequenceService;
    private readonly WarehouseService _warehouseService;
    private readonly IQueryContext _queryContext;
    private readonly ICommandRepository<InventoryTransaction> _inventoryTransactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InventoryTransactionService(
        NumberSequenceService numberSequenceService,
        WarehouseService warehouseService,
        IQueryContext queryContext,
        ICommandRepository<InventoryTransaction> inventoryTransactionRepository,
        IUnitOfWork unitOfWork
        )
    {
        _numberSequenceService = numberSequenceService;
        _warehouseService = warehouseService;
        _queryContext = queryContext;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _unitOfWork = unitOfWork;
    }

    public double GetStock(string? warehouseId, string? productId, string? currentId = null)
    {
        var result = 0.0;
        if (currentId == null)
        {
            result = _queryContext.InventoryTransaction
                .ApplyIsDeletedFilter(false)
                .Include(x => x.Product)
                .Where(x =>
                    x.Status == InventoryTransactionStatus.Confirmed &&
                    x.WarehouseId == warehouseId &&
                    x.ProductId == productId &&
                    x.Product!.Physical == true)
                .Sum(x => x.Stock ?? 0.0);

        }
        else
        {

            result = _queryContext.InventoryTransaction
                .ApplyIsDeletedFilter(false)
                .Include(x => x.Product)
                .Where(x =>
                    x.Status == InventoryTransactionStatus.Confirmed &&
                    x.WarehouseId == warehouseId &&
                    x.ProductId == productId &&
                    x.Product!.Physical == true &&
                    x.Id != currentId)
                .Sum(x => x.Stock ?? 0.0);
        }
        return result;
    }

    public async Task PropagateParentUpdate(
        string? moduleId,
        string? moduleName,
        DateTime? movementDate,
        InventoryTransactionStatus? status,
        bool? isDeleted,
        string? updatedId,
        string? warehouseId = null,
        CancellationToken cancellationToken = default
        )
    {
        var childs = await _inventoryTransactionRepository
            .GetQuery()
            .Where(x => x.ModuleId == moduleId && x.ModuleName == moduleName)
            .ToListAsync(cancellationToken);

        foreach (var item in childs)
        {
            item.MovementDate = movementDate;
            item.Status = status;
            item.IsDeleted = isDeleted ?? false;
            item.UpdatedById = updatedId;
            item.UpdatedAtUtc = DateTime.UtcNow;
            if (warehouseId != null)
            {
                item.WarehouseId = warehouseId;
            }
        }

        await _unitOfWork.SaveAsync(cancellationToken);
    }


    public InventoryTransaction CalculateInvenTrans(InventoryTransaction? transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        var moduleName = transaction.ModuleName;

        if (moduleName != nameof(StockCount) && transaction.Movement <= 0.0)
        {
            throw new Exception("Quantity must not zero and should be positive.");
        }

        if (moduleName == nameof(StockCount) && transaction.QtySCCount <= 0.0)
        {
            throw new Exception("Quantity must not zero and should be positive.");
        }

        switch (moduleName)
        {
            case nameof(DeliveryOrder):
                DeliveryOrderProcessing(transaction);
                break;
            case nameof(GoodsReceive):
                GoodsReceiveProcessing(transaction);
                break;
            case nameof(SalesReturn):
                SalesReturnProcessing(transaction);
                break;
            case nameof(PurchaseReturn):
                PurchaseReturnProcessing(transaction);
                break;
            case nameof(TransferIn):
                TransferInProcessing(transaction);
                break;
            case nameof(TransferOut):
                TransferOutProcessing(transaction);
                break;
            case nameof(StockCount):
                StockCountProcessing(transaction);
                break;
            case nameof(NegativeAdjustment):
                AdjustmentMinusProcessing(transaction);
                break;
            case nameof(PositiveAdjustment):
                AdjustmentPlusProcessing(transaction);
                break;
            case nameof(Scrapping):
                ScrappingProcessing(transaction);
                break;
            default:
                break;
        }

        return transaction;
    }

    private void CalculateStock(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.Stock = transaction.Movement * (int)(transaction.TransType ?? 0.0);
    }

    private InventoryTransaction DeliveryOrderProcessing(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.TransType = InventoryTransType.Out;
        CalculateStock(transaction);
        transaction.WarehouseFromId = transaction.WarehouseId;
        transaction.WarehouseToId = _warehouseService.GetCustomerWarehouse()!.Id;

        return transaction;
    }

    private InventoryTransaction GoodsReceiveProcessing(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.TransType = InventoryTransType.In;
        CalculateStock(transaction);
        transaction.WarehouseFromId = _warehouseService.GetVendorWarehouse()!.Id;
        transaction.WarehouseToId = transaction.WarehouseId;

        return transaction;
    }

    private InventoryTransaction SalesReturnProcessing(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.TransType = InventoryTransType.In;
        CalculateStock(transaction);
        transaction.WarehouseFromId = _warehouseService.GetVendorWarehouse()!.Id;
        transaction.WarehouseToId = transaction.WarehouseId;

        return transaction;
    }

    private InventoryTransaction PurchaseReturnProcessing(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.TransType = InventoryTransType.Out;
        CalculateStock(transaction);
        transaction.WarehouseFromId = transaction.WarehouseId;
        transaction.WarehouseToId = _warehouseService.GetCustomerWarehouse()!.Id;

        return transaction;
    }

    private InventoryTransaction TransferInProcessing(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.TransType = InventoryTransType.In;
        CalculateStock(transaction);
        transaction.WarehouseFromId = _warehouseService.GetTransferWarehouse()!.Id;
        transaction.WarehouseToId = transaction.WarehouseId;

        return transaction;
    }

    private InventoryTransaction TransferOutProcessing(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.TransType = InventoryTransType.Out;
        CalculateStock(transaction);
        transaction.WarehouseFromId = transaction.WarehouseId;
        transaction.WarehouseToId = _warehouseService.GetTransferWarehouse()!.Id;

        return transaction;
    }

    private InventoryTransaction StockCountProcessing(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.QtySCSys = GetStock(transaction.WarehouseId, transaction.ProductId, transaction.Id);
        transaction.QtySCDelta = transaction.QtySCSys - transaction.QtySCCount;
        transaction.Movement = Math.Abs(transaction.QtySCDelta ?? 0.0);

        if (transaction.QtySCDelta < 0.0)
        {

            transaction.TransType = InventoryTransType.In;
            CalculateStock(transaction);
            transaction.WarehouseFromId = _warehouseService.GetStockCountWarehouse()!.Id;
            transaction.WarehouseToId = transaction.WarehouseId;

        }
        else
        {

            transaction.TransType = InventoryTransType.Out;
            CalculateStock(transaction);
            transaction.WarehouseFromId = transaction.WarehouseId;
            transaction.WarehouseToId = _warehouseService.GetStockCountWarehouse()!.Id;

        }

        return transaction;
    }

    private InventoryTransaction AdjustmentMinusProcessing(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.TransType = InventoryTransType.Out;
        CalculateStock(transaction);
        transaction.WarehouseFromId = transaction.WarehouseId;
        transaction.WarehouseToId = _warehouseService.GetAdjustmentWarehouse()!.Id;

        return transaction;
    }

    private InventoryTransaction AdjustmentPlusProcessing(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.TransType = InventoryTransType.In;
        CalculateStock(transaction);
        transaction.WarehouseFromId = _warehouseService.GetAdjustmentWarehouse()!.Id;
        transaction.WarehouseToId = transaction.WarehouseId;

        return transaction;
    }

    private InventoryTransaction ScrappingProcessing(InventoryTransaction transaction)
    {
        if (transaction == null)
        {
            throw new Exception("Inventory transaction is null");
        }

        transaction.TransType = InventoryTransType.Out;
        CalculateStock(transaction);
        transaction.WarehouseFromId = transaction.WarehouseId;
        transaction.WarehouseToId = _warehouseService.GetScrappingWarehouse()!.Id;

        return transaction;
    }

}
