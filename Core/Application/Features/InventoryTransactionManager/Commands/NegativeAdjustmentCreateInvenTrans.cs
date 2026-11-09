using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class NegativeAdjustmentCreateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class NegativeAdjustmentCreateInvenTransRequest : IRequest<NegativeAdjustmentCreateInvenTransResult>
{
    public string? ModuleId { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? CreatedById { get; init; }
}

public class NegativeAdjustmentCreateInvenTransValidator : AbstractValidator<NegativeAdjustmentCreateInvenTransRequest>
{
    public NegativeAdjustmentCreateInvenTransValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.CreatedById).NotEmpty();
    }
}

public class NegativeAdjustmentCreateInvenTransHandler : IRequestHandler<NegativeAdjustmentCreateInvenTransRequest, NegativeAdjustmentCreateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public NegativeAdjustmentCreateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<NegativeAdjustmentCreateInvenTransResult> Handle(NegativeAdjustmentCreateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.NegativeAdjustmentCreateInvenTrans(
            request.ModuleId,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.CreatedById,
            cancellationToken);

        return new NegativeAdjustmentCreateInvenTransResult
        {
            Data = entity
        };
    }
}