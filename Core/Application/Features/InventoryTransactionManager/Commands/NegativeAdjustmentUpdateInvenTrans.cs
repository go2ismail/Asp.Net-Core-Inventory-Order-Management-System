using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class NegativeAdjustmentUpdateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class NegativeAdjustmentUpdateInvenTransRequest : IRequest<NegativeAdjustmentUpdateInvenTransResult>
{
    public string? Id { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? UpdatedById { get; init; }

}

public class NegativeAdjustmentUpdateInvenTransValidator : AbstractValidator<NegativeAdjustmentUpdateInvenTransRequest>
{
    public NegativeAdjustmentUpdateInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class NegativeAdjustmentUpdateInvenTransHandler : IRequestHandler<NegativeAdjustmentUpdateInvenTransRequest, NegativeAdjustmentUpdateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public NegativeAdjustmentUpdateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<NegativeAdjustmentUpdateInvenTransResult> Handle(NegativeAdjustmentUpdateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.NegativeAdjustmentUpdateInvenTrans(
            request.Id,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.UpdatedById,
            cancellationToken);

        return new NegativeAdjustmentUpdateInvenTransResult
        {
            Data = entity
        };
    }
}