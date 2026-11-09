using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class PositiveAdjustmentDeleteInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class PositiveAdjustmentDeleteInvenTransRequest : IRequest<PositiveAdjustmentDeleteInvenTransResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }

}

public class PositiveAdjustmentDeleteInvenTransValidator : AbstractValidator<PositiveAdjustmentDeleteInvenTransRequest>
{
    public PositiveAdjustmentDeleteInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeletedById).NotEmpty();
    }
}

public class PositiveAdjustmentDeleteInvenTransHandler : IRequestHandler<PositiveAdjustmentDeleteInvenTransRequest, PositiveAdjustmentDeleteInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public PositiveAdjustmentDeleteInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<PositiveAdjustmentDeleteInvenTransResult> Handle(PositiveAdjustmentDeleteInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.PositiveAdjustmentDeleteInvenTrans(
            request.Id,
            request.DeletedById,
            cancellationToken);

        return new PositiveAdjustmentDeleteInvenTransResult
        {
            Data = entity
        };
    }
}