using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class PositiveAdjustmentCreateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class PositiveAdjustmentCreateInvenTransRequest : IRequest<PositiveAdjustmentCreateInvenTransResult>
{
    public string? ModuleId { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? CreatedById { get; init; }
}

public class PositiveAdjustmentCreateInvenTransValidator : AbstractValidator<PositiveAdjustmentCreateInvenTransRequest>
{
    public PositiveAdjustmentCreateInvenTransValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.CreatedById).NotEmpty();
    }
}

public class PositiveAdjustmentCreateInvenTransHandler : IRequestHandler<PositiveAdjustmentCreateInvenTransRequest, PositiveAdjustmentCreateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public PositiveAdjustmentCreateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<PositiveAdjustmentCreateInvenTransResult> Handle(PositiveAdjustmentCreateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.PositiveAdjustmentCreateInvenTrans(
            request.ModuleId,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.CreatedById,
            cancellationToken);

        return new PositiveAdjustmentCreateInvenTransResult
        {
            Data = entity
        };
    }
}