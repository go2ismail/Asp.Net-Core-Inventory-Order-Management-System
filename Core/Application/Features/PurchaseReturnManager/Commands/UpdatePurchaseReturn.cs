using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.PurchaseReturnManager.Commands;

public class UpdatePurchaseReturnResult
{
    public PurchaseReturn? Data { get; set; }
}

public class UpdatePurchaseReturnRequest : IRequest<UpdatePurchaseReturnResult>
{
    public string? Id { get; init; }
    public DateTime? ReturnDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? GoodsReceiveId { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdatePurchaseReturnValidator : AbstractValidator<UpdatePurchaseReturnRequest>
{
    public UpdatePurchaseReturnValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ReturnDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.GoodsReceiveId).NotEmpty();
    }
}

public class UpdatePurchaseReturnHandler : IRequestHandler<UpdatePurchaseReturnRequest, UpdatePurchaseReturnResult>
{
    private readonly ICommandRepository<PurchaseReturn> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public UpdatePurchaseReturnHandler(
        ICommandRepository<PurchaseReturn> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<UpdatePurchaseReturnResult> Handle(UpdatePurchaseReturnRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.ReturnDate = request.ReturnDate;
        entity.Status = (PurchaseReturnStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.GoodsReceiveId = request.GoodsReceiveId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        await _inventoryTransactionService.PropagateParentUpdate(
            entity.Id,
            nameof(PurchaseReturn),
            entity.ReturnDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            null,
            cancellationToken
            );

        return new UpdatePurchaseReturnResult
        {
            Data = entity
        };
    }
}

