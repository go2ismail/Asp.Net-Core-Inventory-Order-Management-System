using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class TransferInSeeder
{
    private readonly ICommandRepository<TransferIn> _transferInRepository;
    private readonly ICommandRepository<TransferOut> _transferOutRepository;
    private readonly ICommandRepository<InventoryTransaction> _inventoryTransactionRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;
    private readonly IUnitOfWork _unitOfWork;

    public TransferInSeeder(
        ICommandRepository<TransferIn> transferInRepository,
        ICommandRepository<TransferOut> transferOutRepository,
        ICommandRepository<InventoryTransaction> inventoryTransactionRepository,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService,
        IUnitOfWork unitOfWork
    )
    {
        _transferInRepository = transferInRepository;
        _transferOutRepository = transferOutRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var random = new Random();
        var transferStatusLength = Enum.GetNames(typeof(TransferStatus)).Length;

        var transferOuts = await _transferOutRepository
            .GetQuery()
            .Where(x => x.Status >= TransferStatus.Confirmed)
            .ToListAsync();

        foreach (var transferOut in transferOuts)
        {
            bool process = random.Next(2) == 0;
            if (process)
            {
                continue;
            }

            var transferIn = new TransferIn
            {
                Number = _numberSequenceService.GenerateNumber(nameof(TransferIn), "", "IN"),
                TransferReceiveDate = transferOut.TransferReleaseDate?.AddDays(random.Next(1, 5)),
                Status = (TransferStatus)random.Next(0, transferStatusLength),
                TransferOutId = transferOut.Id,
            };
            await _transferInRepository.CreateAsync(transferIn);

            var items = await _inventoryTransactionRepository
                .GetQuery()
                .Where(x => x.ModuleId == transferOut.Id && x.ModuleName == nameof(TransferOut))
                .ToListAsync();

            foreach (var item in items)
            {
                var inventoryTransaction = new InventoryTransaction
                {
                    ModuleId = transferIn.Id,
                    ModuleName = nameof(TransferIn),
                    ModuleCode = "TO-IN",
                    ModuleNumber = transferIn.Number,
                    MovementDate = transferIn.TransferReceiveDate!.Value,
                    Status = (InventoryTransactionStatus)transferIn.Status,
                    Number = _numberSequenceService.GenerateNumber(nameof(InventoryTransaction), "", "IVT"),
                    WarehouseId = transferOut.WarehouseToId,
                    ProductId = item.ProductId,
                    Movement = item.Movement
                };

                _inventoryTransactionService.CalculateInvenTrans(inventoryTransaction);
                await _inventoryTransactionRepository.CreateAsync(inventoryTransaction);
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
