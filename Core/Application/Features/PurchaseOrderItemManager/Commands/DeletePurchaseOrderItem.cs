using Application.Common.Repositories;
using Application.Features.PurchaseOrderManager;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.PurchaseOrderItemManager.Commands;

public class DeletePurchaseOrderItemResult
{
    public PurchaseOrderItem? Data { get; set; }
}

public class DeletePurchaseOrderItemRequest : IRequest<DeletePurchaseOrderItemResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeletePurchaseOrderItemValidator : AbstractValidator<DeletePurchaseOrderItemRequest>
{
    public DeletePurchaseOrderItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeletePurchaseOrderItemHandler : IRequestHandler<DeletePurchaseOrderItemRequest, DeletePurchaseOrderItemResult>
{
    private readonly ICommandRepository<PurchaseOrderItem> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PurchaseOrderService _purchaseOrderService;

    public DeletePurchaseOrderItemHandler(
        ICommandRepository<PurchaseOrderItem> repository,
        IUnitOfWork unitOfWork,
        PurchaseOrderService purchaseOrderService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _purchaseOrderService = purchaseOrderService;
    }

    public async Task<DeletePurchaseOrderItemResult> Handle(DeletePurchaseOrderItemRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        _purchaseOrderService.Recalculate(entity.PurchaseOrderId ?? "");

        return new DeletePurchaseOrderItemResult
        {
            Data = entity
        };
    }
}

