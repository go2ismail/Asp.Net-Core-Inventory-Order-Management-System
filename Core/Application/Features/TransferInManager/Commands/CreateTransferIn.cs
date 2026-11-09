using Application.Common.Extensions;
using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TransferInManager.Commands;

public class CreateTransferInResult
{
    public TransferIn? Data { get; set; }
}

public class CreateTransferInRequest : IRequest<CreateTransferInResult>
{
    public DateTime? TransferReceiveDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? TransferOutId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateTransferInValidator : AbstractValidator<CreateTransferInRequest>
{
    public CreateTransferInValidator()
    {
        RuleFor(x => x.TransferReceiveDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.TransferOutId).NotEmpty();
    }
}

public class CreateTransferInHandler : IRequestHandler<CreateTransferInRequest, CreateTransferInResult>
{
    private readonly ICommandRepository<TransferIn> _deliveryOrderRepository;
    private readonly ICommandRepository<InventoryTransaction> _itemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public CreateTransferInHandler(
        ICommandRepository<TransferIn> deliveryOrderRepository,
        ICommandRepository<InventoryTransaction> itemRepository,
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

    public async Task<CreateTransferInResult> Handle(CreateTransferInRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new TransferIn();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(TransferIn), "", "IN");
        entity.TransferReceiveDate = request.TransferReceiveDate;
        entity.Status = (TransferStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.TransferOutId = request.TransferOutId;

        await _deliveryOrderRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var items = await _itemRepository
            .GetQuery()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.ModuleId == entity.TransferOutId && x.ModuleName == nameof(TransferOut))
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            if (item?.Product?.Physical ?? false)
            {
                await _inventoryTransactionService.TransferInCreateInvenTrans(
                    entity.Id,
                    item.ProductId,
                    item.Movement,
                    entity.CreatedById,
                    cancellationToken
                    );

            }
        }

        return new CreateTransferInResult
        {
            Data = entity
        };
    }
}