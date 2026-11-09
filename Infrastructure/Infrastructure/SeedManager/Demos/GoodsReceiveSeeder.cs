using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class GoodsReceiveSeeder
{
    private readonly ICommandRepository<GoodsReceive> _goodsReceiveRepository;
    private readonly ICommandRepository<PurchaseOrder> _purchaseOrderRepository;
    private readonly ICommandRepository<PurchaseOrderItem> _purchaseOrderItemRepository;
    private readonly ICommandRepository<Warehouse> _warehouseRepository;
    private readonly ICommandRepository<InventoryTransaction> _inventoryTransactionRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;
    private readonly IUnitOfWork _unitOfWork;

    public GoodsReceiveSeeder(
        ICommandRepository<GoodsReceive> goodsReceiveRepository,
        ICommandRepository<PurchaseOrder> purchaseOrderRepository,
        ICommandRepository<PurchaseOrderItem> purchaseOrderItemRepository,
        ICommandRepository<Warehouse> warehouseRepository,
        ICommandRepository<InventoryTransaction> inventoryTransactionRepository,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService,
        IUnitOfWork unitOfWork
    )
    {
        _goodsReceiveRepository = goodsReceiveRepository;
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _warehouseRepository = warehouseRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var random = new Random();
        var goodsReceiveStatusLength = Enum.GetNames(typeof(GoodsReceiveStatus)).Length;

        var purchaseOrders = await _purchaseOrderRepository
            .GetQuery()
            .Where(x => x.OrderStatus >= PurchaseOrderStatus.Confirmed)
            .ToListAsync();

        var warehouses = await _warehouseRepository
            .GetQuery()
            .Where(x => x.SystemWarehouse == false)
            .Select(x => x.Id)
            .ToListAsync();

        foreach (var purchaseOrder in purchaseOrders)
        {
            var goodsReceive = new GoodsReceive
            {
                Number = _numberSequenceService.GenerateNumber(nameof(GoodsReceive), "", "GR"),
                ReceiveDate = purchaseOrder.OrderDate?.AddDays(random.Next(1, 5)),
                Status = (GoodsReceiveStatus)random.Next(0, goodsReceiveStatusLength),
                PurchaseOrderId = purchaseOrder.Id,
            };
            await _goodsReceiveRepository.CreateAsync(goodsReceive);

            var items = await _purchaseOrderItemRepository
                .GetQuery()
                .Include(x => x.Product)
                .Where(x => x.PurchaseOrderId == purchaseOrder.Id && x.Product!.Physical == true)
                .ToListAsync();

            foreach (var item in items)
            {
                var inventoryTransaction = new InventoryTransaction
                {
                    ModuleId = goodsReceive.Id,
                    ModuleName = nameof(GoodsReceive),
                    ModuleCode = "GR",
                    ModuleNumber = goodsReceive.Number,
                    MovementDate = goodsReceive.ReceiveDate!.Value,
                    Status = (InventoryTransactionStatus)goodsReceive.Status,
                    Number = _numberSequenceService.GenerateNumber(nameof(InventoryTransaction), "", "IVT"),
                    WarehouseId = GetRandomValue(warehouses, random),
                    ProductId = item.ProductId,
                    Movement = item.Quantity!.Value
                };

                _inventoryTransactionService.CalculateInvenTrans(inventoryTransaction);

                await _inventoryTransactionRepository.CreateAsync(inventoryTransaction);
            }

            await _unitOfWork.SaveAsync();
        }
    }

    private static T GetRandomValue<T>(List<T> list, Random random)
    {
        return list[random.Next(list.Count)];
    }
}
