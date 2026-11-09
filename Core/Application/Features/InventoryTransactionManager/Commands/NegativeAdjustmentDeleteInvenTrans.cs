using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class NegativeAdjustmentDeleteInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class NegativeAdjustmentDeleteInvenTransRequest : IRequest<NegativeAdjustmentDeleteInvenTransResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }

}

public class NegativeAdjustmentDeleteInvenTransValidator : AbstractValidator<NegativeAdjustmentDeleteInvenTransRequest>
{
    public NegativeAdjustmentDeleteInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeletedById).NotEmpty();
    }
}

public class NegativeAdjustmentDeleteInvenTransHandler : IRequestHandler<NegativeAdjustmentDeleteInvenTransRequest, NegativeAdjustmentDeleteInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public NegativeAdjustmentDeleteInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<NegativeAdjustmentDeleteInvenTransResult> Handle(NegativeAdjustmentDeleteInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.NegativeAdjustmentDeleteInvenTrans(
            request.Id,
            request.DeletedById,
            cancellationToken);

        return new NegativeAdjustmentDeleteInvenTransResult
        {
            Data = entity
        };
    }
}