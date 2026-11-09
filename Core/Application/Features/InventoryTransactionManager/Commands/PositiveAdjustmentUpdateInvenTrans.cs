using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class PositiveAdjustmentUpdateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class PositiveAdjustmentUpdateInvenTransRequest : IRequest<PositiveAdjustmentUpdateInvenTransResult>
{
    public string? Id { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? UpdatedById { get; init; }

}

public class PositiveAdjustmentUpdateInvenTransValidator : AbstractValidator<PositiveAdjustmentUpdateInvenTransRequest>
{
    public PositiveAdjustmentUpdateInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class PositiveAdjustmentUpdateInvenTransHandler : IRequestHandler<PositiveAdjustmentUpdateInvenTransRequest, PositiveAdjustmentUpdateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public PositiveAdjustmentUpdateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<PositiveAdjustmentUpdateInvenTransResult> Handle(PositiveAdjustmentUpdateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.PositiveAdjustmentUpdateInvenTrans(
            request.Id,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.UpdatedById,
            cancellationToken);

        return new PositiveAdjustmentUpdateInvenTransResult
        {
            Data = entity
        };
    }
}