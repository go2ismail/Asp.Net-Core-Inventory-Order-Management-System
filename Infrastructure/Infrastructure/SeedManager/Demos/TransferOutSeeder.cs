using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class TransferOutSeeder
{
    private readonly ICommandRepository<TransferOut> _transferOutRepository;
    private readonly ICommandRepository<Warehouse> _warehouseRepository;
    private readonly ICommandRepository<InventoryTransaction> _inventoryTransactionRepository;
    private readonly ICommandRepository<Product> _productRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;
    private readonly IUnitOfWork _unitOfWork;

    public TransferOutSeeder(
        ICommandRepository<TransferOut> transferOutRepository,
        ICommandRepository<Warehouse> warehouseRepository,
        ICommandRepository<InventoryTransaction> inventoryTransactionRepository,
        ICommandRepository<Product> productRepository,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService,
        IUnitOfWork unitOfWork
    )
    {
        _transferOutRepository = transferOutRepository;
        _warehouseRepository = warehouseRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _productRepository = productRepository;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var random = new Random();
        var transferStatusLength = Enum.GetNames(typeof(TransferStatus)).Length;

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
            var transactionDates = GetRandomDays(date.Year, date.Month, 6);

            foreach (DateTime transDate in transactionDates)
            {
                var fromId = GetRandomValue(warehouses, random);
                var toId = GetRandomValue(warehouses.Where(x => x != fromId).ToList(), random);

                var transferOut = new TransferOut
                {
                    Number = _numberSequenceService.GenerateNumber(nameof(TransferOut), "", "OUT"),
                    TransferReleaseDate = transDate,
                    Status = (TransferStatus)random.Next(0, transferStatusLength),
                    WarehouseFromId = fromId,
                    WarehouseToId = toId,
                };
                await _transferOutRepository.CreateAsync(transferOut);

                int numberOfProducts = random.Next(3, 6);
                for (int i = 0; i < numberOfProducts; i++)
                {
                    var product = GetRandomValue(products, random);

                    var inventoryTransaction = new InventoryTransaction
                    {
                        ModuleId = transferOut.Id,
                        ModuleName = nameof(TransferOut),
                        ModuleCode = "TO-OUT",
                        ModuleNumber = transferOut.Number,
                        MovementDate = transferOut.TransferReleaseDate!.Value,
                        Status = (InventoryTransactionStatus)transferOut.Status,
                        Number = _numberSequenceService.GenerateNumber(nameof(InventoryTransaction), "", "IVT"),
                        WarehouseId = transferOut.WarehouseFromId,
                        ProductId = product.Id,
                        Movement = random.Next(1, 10)
                    };

                    _inventoryTransactionService.CalculateInvenTrans(inventoryTransaction);
                    await _inventoryTransactionRepository.CreateAsync(inventoryTransaction);
                }

                await _unitOfWork.SaveAsync();
            }
        }
    }

    private static T GetRandomValue<T>(List<T> list, Random random)
    {
        return list[random.Next(list.Count)];
    }

    private static DateTime[] GetRandomDays(int year, int month, int count)
    {
        var random = new Random();
        var daysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month)).ToArray();
        return daysInMonth.OrderBy(x => random.Next()).Take(count).Select(day => new DateTime(year, month, day)).ToArray();
    }
}
