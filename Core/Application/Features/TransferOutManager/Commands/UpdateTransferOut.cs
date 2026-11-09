using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.TransferOutManager.Commands;

public class UpdateTransferOutResult
{
    public TransferOut? Data { get; set; }
}

public class UpdateTransferOutRequest : IRequest<UpdateTransferOutResult>
{
    public string? Id { get; init; }
    public DateTime? TransferReleaseDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? WarehouseFromId { get; init; }
    public string? WarehouseToId { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateTransferOutValidator : AbstractValidator<UpdateTransferOutRequest>
{
    public UpdateTransferOutValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TransferReleaseDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.WarehouseFromId).NotEmpty();
        RuleFor(x => x.WarehouseToId).NotEmpty();
    }
}

public class UpdateTransferOutHandler : IRequestHandler<UpdateTransferOutRequest, UpdateTransferOutResult>
{
    private readonly ICommandRepository<TransferOut> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public UpdateTransferOutHandler(
        ICommandRepository<TransferOut> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<UpdateTransferOutResult> Handle(UpdateTransferOutRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.TransferReleaseDate = request.TransferReleaseDate;
        entity.Status = (TransferStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.WarehouseFromId = request.WarehouseFromId;
        entity.WarehouseToId = request.WarehouseToId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        await _inventoryTransactionService.PropagateParentUpdate(
            entity.Id,
            nameof(TransferOut),
            entity.TransferReleaseDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            entity.WarehouseFromId,
            cancellationToken
            );

        return new UpdateTransferOutResult
        {
            Data = entity
        };
    }
}

