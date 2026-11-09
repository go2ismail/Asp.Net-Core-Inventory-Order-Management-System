using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.PurchaseReturnManager.Commands;

public class DeletePurchaseReturnResult
{
    public PurchaseReturn? Data { get; set; }
}

public class DeletePurchaseReturnRequest : IRequest<DeletePurchaseReturnResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeletePurchaseReturnValidator : AbstractValidator<DeletePurchaseReturnRequest>
{
    public DeletePurchaseReturnValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeletePurchaseReturnHandler : IRequestHandler<DeletePurchaseReturnRequest, DeletePurchaseReturnResult>
{
    private readonly ICommandRepository<PurchaseReturn> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeletePurchaseReturnHandler(
        ICommandRepository<PurchaseReturn> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeletePurchaseReturnResult> Handle(DeletePurchaseReturnRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
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

        return new DeletePurchaseReturnResult
        {
            Data = entity
        };
    }
}

