using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class DeliveryOrderDeleteInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class DeliveryOrderDeleteInvenTransRequest : IRequest<DeliveryOrderDeleteInvenTransResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }

}

public class DeliveryOrderDeleteInvenTransValidator : AbstractValidator<DeliveryOrderDeleteInvenTransRequest>
{
    public DeliveryOrderDeleteInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeletedById).NotEmpty();
    }
}

public class DeliveryOrderDeleteInvenTransHandler : IRequestHandler<DeliveryOrderDeleteInvenTransRequest, DeliveryOrderDeleteInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeliveryOrderDeleteInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeliveryOrderDeleteInvenTransResult> Handle(DeliveryOrderDeleteInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.DeliveryOrderDeleteInvenTrans(
            request.Id,
            request.DeletedById,
            cancellationToken);

        return new DeliveryOrderDeleteInvenTransResult
        {
            Data = entity
        };
    }
}