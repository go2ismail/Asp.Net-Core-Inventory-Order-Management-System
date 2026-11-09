using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class SalesReturnSeeder
{
    private readonly ICommandRepository<SalesReturn> _salesReturnRepository;
    private readonly ICommandRepository<DeliveryOrder> _deliveryOrderRepository;
    private readonly ICommandRepository<Warehouse> _warehouseRepository;
    private readonly ICommandRepository<InventoryTransaction> _inventoryTransactionRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;
    private readonly IUnitOfWork _unitOfWork;

    public SalesReturnSeeder(
        ICommandRepository<SalesReturn> salesReturnRepository,
        ICommandRepository<DeliveryOrder> deliveryOrderRepository,
        ICommandRepository<Warehouse> warehouseRepository,
        ICommandRepository<InventoryTransaction> inventoryTransactionRepository,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService,
        IUnitOfWork unitOfWork
    )
    {
        _salesReturnRepository = salesReturnRepository;
        _deliveryOrderRepository = deliveryOrderRepository;
        _warehouseRepository = warehouseRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var random = new Random();
        var salesReturnStatusLength = Enum.GetNames(typeof(SalesReturnStatus)).Length;

        var deliveryOrders = await _deliveryOrderRepository
            .GetQuery()
            .Where(x => x.Status >= DeliveryOrderStatus.Confirmed)
            .ToListAsync();

        var warehouses = await _warehouseRepository
            .GetQuery()
            .Where(x => x.SystemWarehouse == false)
            .Select(x => x.Id)
            .ToListAsync();

        foreach (var deliveryOrder in deliveryOrders)
        {
            bool process = random.Next(2) == 0;

            if (process)
            {
                continue;
            }

            var salesReturn = new SalesReturn
            {
                Number = _numberSequenceService.GenerateNumber(nameof(SalesReturn), "", "SRN"),
                ReturnDate = deliveryOrder.DeliveryDate?.AddDays(random.Next(1, 5)),
                Status = (SalesReturnStatus)random.Next(0, salesReturnStatusLength),
                DeliveryOrderId = deliveryOrder.Id,
            };
            await _salesReturnRepository.CreateAsync(salesReturn);

            var items = await _inventoryTransactionRepository
                .GetQuery()
                .Where(x => x.ModuleId == deliveryOrder.Id && x.ModuleName == nameof(DeliveryOrder))
                .ToListAsync();

            foreach (var item in items)
            {
                var inventoryTransaction = new InventoryTransaction
                {
                    ModuleId = salesReturn.Id,
                    ModuleName = nameof(SalesReturn),
                    ModuleCode = "SRN",
                    ModuleNumber = salesReturn.Number,
                    MovementDate = salesReturn.ReturnDate!.Value,
                    Status = (InventoryTransactionStatus)salesReturn.Status,
                    Number = _numberSequenceService.GenerateNumber(nameof(InventoryTransaction), "", "IVT"),
                    WarehouseId = GetRandomValue(warehouses, random),
                    ProductId = item.ProductId,
                    Movement = item.Movement
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
