using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class PurchaseReturnSeeder
{
    private readonly ICommandRepository<PurchaseReturn> _purchaseReturnRepository;
    private readonly ICommandRepository<GoodsReceive> _goodsReceiveRepository;
    private readonly ICommandRepository<Warehouse> _warehouseRepository;
    private readonly ICommandRepository<InventoryTransaction> _inventoryTransactionRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseReturnSeeder(
        ICommandRepository<PurchaseReturn> purchaseReturnRepository,
        ICommandRepository<GoodsReceive> goodsReceiveRepository,
        ICommandRepository<Warehouse> warehouseRepository,
        ICommandRepository<InventoryTransaction> inventoryTransactionRepository,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService,
        IUnitOfWork unitOfWork
    )
    {
        _purchaseReturnRepository = purchaseReturnRepository;
        _goodsReceiveRepository = goodsReceiveRepository;
        _warehouseRepository = warehouseRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var random = new Random();
        var purchaseReturnStatusLength = Enum.GetNames(typeof(PurchaseReturnStatus)).Length;

        var goodsReceives = await _goodsReceiveRepository
            .GetQuery()
            .Where(x => x.Status >= GoodsReceiveStatus.Confirmed)
            .ToListAsync();

        var warehouses = await _warehouseRepository
            .GetQuery()
            .Where(x => x.SystemWarehouse == false)
            .Select(x => x.Id)
            .ToListAsync();

        foreach (var goodsReceive in goodsReceives)
        {
            bool process = random.Next(2) == 0;

            if (process)
            {
                continue;
            }

            var purchaseReturn = new PurchaseReturn
            {
                Number = _numberSequenceService.GenerateNumber(nameof(PurchaseReturn), "", "PRN"),
                ReturnDate = goodsReceive.ReceiveDate?.AddDays(random.Next(1, 5)),
                Status = (PurchaseReturnStatus)random.Next(0, purchaseReturnStatusLength),
                GoodsReceiveId = goodsReceive.Id,
            };
            await _purchaseReturnRepository.CreateAsync(purchaseReturn);

            var items = await _inventoryTransactionRepository
                .GetQuery()
                .Where(x => x.ModuleId == goodsReceive.Id && x.ModuleName == nameof(GoodsReceive))
                .ToListAsync();

            foreach (var item in items)
            {
                var inventoryTransaction = new InventoryTransaction
                {
                    ModuleId = purchaseReturn.Id,
                    ModuleName = nameof(PurchaseReturn),
                    ModuleCode = "PRN",
                    ModuleNumber = purchaseReturn.Number,
                    MovementDate = purchaseReturn.ReturnDate!.Value,
                    Status = (InventoryTransactionStatus)purchaseReturn.Status,
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
