using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Queries;

public class DeliveryOrderGetInvenTransListResult
{
    public List<InventoryTransaction>? Data { get; set; }
}

public class DeliveryOrderGetInvenTransListRequest : IRequest<DeliveryOrderGetInvenTransListResult>
{
    public string? ModuleId { get; init; }

}

public class DeliveryOrderGetInvenTransListValidator : AbstractValidator<DeliveryOrderGetInvenTransListRequest>
{
    public DeliveryOrderGetInvenTransListValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
    }
}

public class DeliveryOrderGetInvenTransListHandler : IRequestHandler<DeliveryOrderGetInvenTransListRequest, DeliveryOrderGetInvenTransListResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeliveryOrderGetInvenTransListHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeliveryOrderGetInvenTransListResult> Handle(DeliveryOrderGetInvenTransListRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.DeliveryOrderGetInvenTransList(
            request.ModuleId,
            nameof(DeliveryOrder),
            cancellationToken);

        return new DeliveryOrderGetInvenTransListResult
        {
            Data = entity
        };
    }
}