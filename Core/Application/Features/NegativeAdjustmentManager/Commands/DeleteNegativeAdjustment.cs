using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.NegativeAdjustmentManager.Commands;

public class DeleteNegativeAdjustmentResult
{
    public NegativeAdjustment? Data { get; set; }
}

public class DeleteNegativeAdjustmentRequest : IRequest<DeleteNegativeAdjustmentResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteNegativeAdjustmentValidator : AbstractValidator<DeleteNegativeAdjustmentRequest>
{
    public DeleteNegativeAdjustmentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteNegativeAdjustmentHandler : IRequestHandler<DeleteNegativeAdjustmentRequest, DeleteNegativeAdjustmentResult>
{
    private readonly ICommandRepository<NegativeAdjustment> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeleteNegativeAdjustmentHandler(
        ICommandRepository<NegativeAdjustment> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeleteNegativeAdjustmentResult> Handle(DeleteNegativeAdjustmentRequest request, CancellationToken cancellationToken)
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
            nameof(NegativeAdjustment),
            entity.AdjustmentDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            null,
            cancellationToken
            );

        return new DeleteNegativeAdjustmentResult
        {
            Data = entity
        };
    }
}

