using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class StockCountSeeder
{
    private readonly ICommandRepository<StockCount> _stockCountRepository;
    private readonly ICommandRepository<InventoryTransaction> _inventoryTransactionRepository;
    private readonly ICommandRepository<Product> _productRepository;
    private readonly ICommandRepository<Warehouse> _warehouseRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;
    private readonly IUnitOfWork _unitOfWork;

    public StockCountSeeder(
        ICommandRepository<StockCount> stockCountRepository,
        ICommandRepository<InventoryTransaction> inventoryTransactionRepository,
        ICommandRepository<Product> productRepository,
        ICommandRepository<Warehouse> warehouseRepository,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService,
        IUnitOfWork unitOfWork
    )
    {
        _stockCountRepository = stockCountRepository;
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
        var stockCountStatusLength = Enum.GetNames(typeof(StockCountStatus)).Length;

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
                var stockCount = new StockCount
                {
                    Number = _numberSequenceService.GenerateNumber(nameof(StockCount), "", "SC"),
                    CountDate = transDate,
                    Status = (StockCountStatus)random.Next(0, stockCountStatusLength),
                    WarehouseId = GetRandomValue(warehouses, random),
                };
                await _stockCountRepository.CreateAsync(stockCount);

                int numberOfProducts = random.Next(3, 6);
                for (int i = 0; i < numberOfProducts; i++)
                {
                    var product = products[random.Next(products.Count)];
                    var stock = _inventoryTransactionService.GetStock(stockCount.WarehouseId, product.Id);
                    var qtyCount = stock + random.Next(-10, 10);
                    if (qtyCount <= 0.0)
                    {
                        if (qtyCount < 0.0)
                        {
                            qtyCount = Math.Abs(qtyCount) * 2.0;
                        }
                        else if (qtyCount == 0.0)
                        {
                            qtyCount = qtyCount + random.Next(40, 50);
                        }
                    }

                    var inventoryTransaction = new InventoryTransaction
                    {
                        ModuleId = stockCount.Id,
                        ModuleName = nameof(StockCount),
                        ModuleCode = "COUNT",
                        ModuleNumber = stockCount.Number,
                        MovementDate = stockCount.CountDate!.Value,
                        Status = (InventoryTransactionStatus)stockCount.Status,
                        Number = _numberSequenceService.GenerateNumber(nameof(InventoryTransaction), "", "IVT"),
                        WarehouseId = stockCount.WarehouseId,
                        ProductId = product.Id,
                        QtySCCount = qtyCount,
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
