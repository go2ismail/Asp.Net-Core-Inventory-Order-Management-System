using Application.Common.Extensions;
using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseReturnManager.Commands;

public class CreatePurchaseReturnResult
{
    public PurchaseReturn? Data { get; set; }
}

public class CreatePurchaseReturnRequest : IRequest<CreatePurchaseReturnResult>
{
    public DateTime? ReturnDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? GoodsReceiveId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreatePurchaseReturnValidator : AbstractValidator<CreatePurchaseReturnRequest>
{
    public CreatePurchaseReturnValidator()
    {
        RuleFor(x => x.ReturnDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.GoodsReceiveId).NotEmpty();
    }
}

public class CreatePurchaseReturnHandler : IRequestHandler<CreatePurchaseReturnRequest, CreatePurchaseReturnResult>
{
    private readonly ICommandRepository<PurchaseReturn> _deliveryOrderRepository;
    private readonly ICommandRepository<InventoryTransaction> _itemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public CreatePurchaseReturnHandler(
        ICommandRepository<PurchaseReturn> deliveryOrderRepository,
        ICommandRepository<InventoryTransaction> itemRepository,
        ICommandRepository<Warehouse> warehouseRepository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _deliveryOrderRepository = deliveryOrderRepository;
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<CreatePurchaseReturnResult> Handle(CreatePurchaseReturnRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new PurchaseReturn();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(PurchaseReturn), "", "PRN");
        entity.ReturnDate = request.ReturnDate;
        entity.Status = (PurchaseReturnStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.GoodsReceiveId = request.GoodsReceiveId;

        await _deliveryOrderRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var items = await _itemRepository
            .GetQuery()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.ModuleId == entity.GoodsReceiveId && x.ModuleName == nameof(GoodsReceive))
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            if (item?.Product?.Physical ?? false)
            {
                await _inventoryTransactionService.PurchaseReturnCreateInvenTrans(
                    entity.Id,
                    item.WarehouseId,
                    item.ProductId,
                    item.Movement,
                    entity.CreatedById,
                    cancellationToken
                    );

            }
        }

        return new CreatePurchaseReturnResult
        {
            Data = entity
        };
    }
}