using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class NegativeAdjustmentSeeder
{
    private readonly ICommandRepository<NegativeAdjustment> _adjustmentMinusRepository;
    private readonly ICommandRepository<InventoryTransaction> _inventoryTransactionRepository;
    private readonly ICommandRepository<Product> _productRepository;
    private readonly ICommandRepository<Warehouse> _warehouseRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;
    private readonly IUnitOfWork _unitOfWork;

    public NegativeAdjustmentSeeder(
        ICommandRepository<NegativeAdjustment> adjustmentMinusRepository,
        ICommandRepository<InventoryTransaction> inventoryTransactionRepository,
        ICommandRepository<Product> productRepository,
        ICommandRepository<Warehouse> warehouseRepository,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService,
        IUnitOfWork unitOfWork
    )
    {
        _adjustmentMinusRepository = adjustmentMinusRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var random = new Random();
        var adjustmentStatusLength = Enum.GetNames(typeof(AdjustmentStatus)).Length;

        var products = await _productRepository
            .GetQuery()
            .Where(x => x.Physical == true)
            .ToListAsync();

        var warehouses = await _warehouseRepository
            .GetQuery()
            .Where(x => x.SystemWarehouse == false)
            .Select(x => x.Id)
            .ToListAsync();

        var dateFinish = DateTime.Now;
        var dateStart = new DateTime(dateFinish.AddMonths(-12).Year, dateFinish.AddMonths(-12).Month, 1);

        for (DateTime date = dateStart; date < dateFinish; date = date.AddMonths(1))
        {
            DateTime[] transactionDates = GetRandomDays(date.Year, date.Month, 6);

            foreach (var transDate in transactionDates)
            {
                var adjustmentMinus = new NegativeAdjustment
                {
                    Number = _numberSequenceService.GenerateNumber(nameof(NegativeAdjustment), "", "ADJ-"),
                    AdjustmentDate = transDate,
                    Status = (AdjustmentStatus)random.Next(0, adjustmentStatusLength),
                };
                await _adjustmentMinusRepository.CreateAsync(adjustmentMinus);

                int numberOfProducts = random.Next(3, 6);
                for (int i = 0; i < numberOfProducts; i++)
                {
                    var product = products[random.Next(products.Count)];

                    var inventoryTransaction = new InventoryTransaction
                    {
                        ModuleId = adjustmentMinus.Id,
                        ModuleName = nameof(NegativeAdjustment),
                        ModuleCode = "ADJ-",
                        ModuleNumber = adjustmentMinus.Number,
                        MovementDate = adjustmentMinus.AdjustmentDate!.Value,
                        Status = (InventoryTransactionStatus)adjustmentMinus.Status,
                        Number = _numberSequenceService.GenerateNumber(nameof(InventoryTransaction), "", "IVT"),
                        WarehouseId = GetRandomValue(warehouses, random),
                        ProductId = product.Id,
                        Movement = random.Next(1, 3)
                    };

                    _inventoryTransactionService.CalculateInvenTrans(inventoryTransaction);
                    await _inventoryTransactionRepository.CreateAsync(inventoryTransaction);
                }
            }
        }

        await _unitOfWork.SaveAsync();
    }

    private static DateTime[] GetRandomDays(int year, int month, int count)
    {
        var random = new Random();
        var daysInMonth = DateTime.DaysInMonth(year, month);
        return Enumerable.Range(1, count)
            .Select(_ => new DateTime(year, month, random.Next(1, daysInMonth + 1)))
            .ToArray();
    }

    private static T GetRandomValue<T>(IList<T> list, Random random)
    {
        return list[random.Next(list.Count)];
    }
}

