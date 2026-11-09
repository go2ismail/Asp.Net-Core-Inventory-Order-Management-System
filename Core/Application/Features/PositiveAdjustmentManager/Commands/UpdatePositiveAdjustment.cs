using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.PositiveAdjustmentManager.Commands;

public class UpdatePositiveAdjustmentResult
{
    public PositiveAdjustment? Data { get; set; }
}

public class UpdatePositiveAdjustmentRequest : IRequest<UpdatePositiveAdjustmentResult>
{
    public string? Id { get; init; }
    public DateTime? AdjustmentDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdatePositiveAdjustmentValidator : AbstractValidator<UpdatePositiveAdjustmentRequest>
{
    public UpdatePositiveAdjustmentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AdjustmentDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}

public class UpdatePositiveAdjustmentHandler : IRequestHandler<UpdatePositiveAdjustmentRequest, UpdatePositiveAdjustmentResult>
{
    private readonly ICommandRepository<PositiveAdjustment> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public UpdatePositiveAdjustmentHandler(
        ICommandRepository<PositiveAdjustment> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<UpdatePositiveAdjustmentResult> Handle(UpdatePositiveAdjustmentRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.AdjustmentDate = request.AdjustmentDate;
        entity.Status = (AdjustmentStatus)int.Parse(request.Status!);
        entity.Description = request.Description;

        _repository.Update(entity);
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

        return new UpdatePositiveAdjustmentResult
        {
            Data = entity
        };
    }
}

