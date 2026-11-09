using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.PositiveAdjustmentManager.Commands;

public class DeletePositiveAdjustmentResult
{
    public PositiveAdjustment? Data { get; set; }
}

public class DeletePositiveAdjustmentRequest : IRequest<DeletePositiveAdjustmentResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeletePositiveAdjustmentValidator : AbstractValidator<DeletePositiveAdjustmentRequest>
{
    public DeletePositiveAdjustmentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeletePositiveAdjustmentHandler : IRequestHandler<DeletePositiveAdjustmentRequest, DeletePositiveAdjustmentResult>
{
    private readonly ICommandRepository<PositiveAdjustment> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeletePositiveAdjustmentHandler(
        ICommandRepository<PositiveAdjustment> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeletePositiveAdjustmentResult> Handle(DeletePositiveAdjustmentRequest request, CancellationToken cancellationToken)
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
            nameof(PositiveAdjustment),
            entity.AdjustmentDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            null,
            cancellationToken
            );

        return new DeletePositiveAdjustmentResult
        {
            Data = entity
        };
    }
}

