using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class DeliveryOrderUpdateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class DeliveryOrderUpdateInvenTransRequest : IRequest<DeliveryOrderUpdateInvenTransResult>
{
    public string? Id { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? UpdatedById { get; init; }

}

public class DeliveryOrderUpdateInvenTransValidator : AbstractValidator<DeliveryOrderUpdateInvenTransRequest>
{
    public DeliveryOrderUpdateInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class DeliveryOrderUpdateInvenTransHandler : IRequestHandler<DeliveryOrderUpdateInvenTransRequest, DeliveryOrderUpdateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeliveryOrderUpdateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeliveryOrderUpdateInvenTransResult> Handle(DeliveryOrderUpdateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.DeliveryOrderUpdateInvenTrans(
            request.Id,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.UpdatedById,
            cancellationToken);

        return new DeliveryOrderUpdateInvenTransResult
        {
            Data = entity
        };
    }
}