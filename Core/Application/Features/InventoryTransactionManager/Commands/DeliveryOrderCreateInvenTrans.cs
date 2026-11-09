using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class DeliveryOrderCreateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class DeliveryOrderCreateInvenTransRequest : IRequest<DeliveryOrderCreateInvenTransResult>
{
    public string? ModuleId { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? CreatedById { get; init; }
}

public class DeliveryOrderCreateInvenTransValidator : AbstractValidator<DeliveryOrderCreateInvenTransRequest>
{
    public DeliveryOrderCreateInvenTransValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.CreatedById).NotEmpty();
    }
}

public class DeliveryOrderCreateInvenTransHandler : IRequestHandler<DeliveryOrderCreateInvenTransRequest, DeliveryOrderCreateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeliveryOrderCreateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeliveryOrderCreateInvenTransResult> Handle(DeliveryOrderCreateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.DeliveryOrderCreateInvenTrans(
            request.ModuleId,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.CreatedById,
            cancellationToken);

        return new DeliveryOrderCreateInvenTransResult
        {
            Data = entity
        };
    }
}