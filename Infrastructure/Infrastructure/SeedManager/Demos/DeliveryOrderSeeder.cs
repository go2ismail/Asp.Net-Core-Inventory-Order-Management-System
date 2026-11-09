using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class DeliveryOrderSeeder
{
    private readonly ICommandRepository<DeliveryOrder> _deliveryOrderRepository;
    private readonly ICommandRepository<SalesOrder> _salesOrderRepository;
    private readonly ICommandRepository<SalesOrderItem> _salesOrderItemRepository;
    private readonly ICommandRepository<Warehouse> _warehouseRepository;
    private readonly ICommandRepository<InventoryTransaction> _inventoryTransactionRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;
    private readonly IUnitOfWork _unitOfWork;

    public DeliveryOrderSeeder(
        ICommandRepository<DeliveryOrder> deliveryOrderRepository,
        ICommandRepository<SalesOrder> salesOrderRepository,
        ICommandRepository<SalesOrderItem> salesOrderItemRepository,
        ICommandRepository<Warehouse> warehouseRepository,
        ICommandRepository<InventoryTransaction> inventoryTransactionRepository,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService,
        IUnitOfWork unitOfWork
    )
    {
        _deliveryOrderRepository = deliveryOrderRepository;
        _salesOrderRepository = salesOrderRepository;
        _salesOrderItemRepository = salesOrderItemRepository;
        _warehouseRepository = warehouseRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var random = new Random();
        var deliveryOrderStatusLength = Enum.GetNames(typeof(DeliveryOrderStatus)).Length;

        var salesOrders = await _salesOrderRepository
            .GetQuery()
            .Where(x => x.OrderStatus >= SalesOrderStatus.Confirmed)
            .ToListAsync();

        var warehouses = await _warehouseRepository
            .GetQuery()
            .Where(x => x.SystemWarehouse == false)
            .Select(x => x.Id)
            .ToListAsync();

        foreach (var salesOrder in salesOrders)
        {
            var deliveryOrder = new DeliveryOrder
            {
                Number = _numberSequenceService.GenerateNumber(nameof(DeliveryOrder), "", "DO"),
                DeliveryDate = salesOrder.OrderDate?.AddDays(random.Next(1, 5)),
                Status = (DeliveryOrderStatus)random.Next(0, deliveryOrderStatusLength),
                SalesOrderId = salesOrder.Id,
            };
            await _deliveryOrderRepository.CreateAsync(deliveryOrder);

            var items = await _salesOrderItemRepository
                .GetQuery()
                .Include(x => x.Product)
                .Where(x => x.SalesOrderId == salesOrder.Id && x.Product!.Physical == true)
                .ToListAsync();

            foreach (var item in items)
            {
                var inventoryTransaction = new InventoryTransaction
                {
                    ModuleId = deliveryOrder.Id,
                    ModuleName = nameof(DeliveryOrder),
                    ModuleCode = "DO",
                    ModuleNumber = deliveryOrder.Number,
                    MovementDate = deliveryOrder.DeliveryDate!.Value,
                    Status = (InventoryTransactionStatus)deliveryOrder.Status,
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
