using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.DeliveryOrderManager.Commands;

public class UpdateDeliveryOrderResult
{
    public DeliveryOrder? Data { get; set; }
}

public class UpdateDeliveryOrderRequest : IRequest<UpdateDeliveryOrderResult>
{
    public string? Id { get; init; }
    public DateTime? DeliveryDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? SalesOrderId { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateDeliveryOrderValidator : AbstractValidator<UpdateDeliveryOrderRequest>
{
    public UpdateDeliveryOrderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeliveryDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.SalesOrderId).NotEmpty();
    }
}

public class UpdateDeliveryOrderHandler : IRequestHandler<UpdateDeliveryOrderRequest, UpdateDeliveryOrderResult>
{
    private readonly ICommandRepository<DeliveryOrder> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public UpdateDeliveryOrderHandler(
        ICommandRepository<DeliveryOrder> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<UpdateDeliveryOrderResult> Handle(UpdateDeliveryOrderRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.DeliveryDate = request.DeliveryDate;
        entity.Status = (DeliveryOrderStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.SalesOrderId = request.SalesOrderId;

        _repository.Update(entity);
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

        return new UpdateDeliveryOrderResult
        {
            Data = entity
        };
    }
}

