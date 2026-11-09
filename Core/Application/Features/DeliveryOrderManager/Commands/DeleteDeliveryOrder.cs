using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.DeliveryOrderManager.Commands;

public class DeleteDeliveryOrderResult
{
    public DeliveryOrder? Data { get; set; }
}

public class DeleteDeliveryOrderRequest : IRequest<DeleteDeliveryOrderResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteDeliveryOrderValidator : AbstractValidator<DeleteDeliveryOrderRequest>
{
    public DeleteDeliveryOrderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteDeliveryOrderHandler : IRequestHandler<DeleteDeliveryOrderRequest, DeleteDeliveryOrderResult>
{
    private readonly ICommandRepository<DeliveryOrder> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeleteDeliveryOrderHandler(
        ICommandRepository<DeliveryOrder> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeleteDeliveryOrderResult> Handle(DeleteDeliveryOrderRequest request, CancellationToken cancellationToken)
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
            nameof(DeliveryOrder),
            entity.DeliveryDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            null,
            cancellationToken
            );

        return new DeleteDeliveryOrderResult
        {
            Data = entity
        };
    }
}

