using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.TransferOutManager.Commands;

public class DeleteTransferOutResult
{
    public TransferOut? Data { get; set; }
}

public class DeleteTransferOutRequest : IRequest<DeleteTransferOutResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteTransferOutValidator : AbstractValidator<DeleteTransferOutRequest>
{
    public DeleteTransferOutValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteTransferOutHandler : IRequestHandler<DeleteTransferOutRequest, DeleteTransferOutResult>
{
    private readonly ICommandRepository<TransferOut> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeleteTransferOutHandler(
        ICommandRepository<TransferOut> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeleteTransferOutResult> Handle(DeleteTransferOutRequest request, CancellationToken cancellationToken)
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
            nameof(TransferOut),
            entity.TransferReleaseDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            null,
            cancellationToken
            );

        return new DeleteTransferOutResult
        {
            Data = entity
        };
    }
}

