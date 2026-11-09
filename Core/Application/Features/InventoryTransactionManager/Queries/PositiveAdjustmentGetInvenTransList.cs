using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Queries;

public class PositiveAdjustmentGetInvenTransListResult
{
    public List<InventoryTransaction>? Data { get; set; }
}

public class PositiveAdjustmentGetInvenTransListRequest : IRequest<PositiveAdjustmentGetInvenTransListResult>
{
    public string? ModuleId { get; init; }

}

public class PositiveAdjustmentGetInvenTransListValidator : AbstractValidator<PositiveAdjustmentGetInvenTransListRequest>
{
    public PositiveAdjustmentGetInvenTransListValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
    }
}

public class PositiveAdjustmentGetInvenTransListHandler : IRequestHandler<PositiveAdjustmentGetInvenTransListRequest, PositiveAdjustmentGetInvenTransListResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public PositiveAdjustmentGetInvenTransListHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<PositiveAdjustmentGetInvenTransListResult> Handle(PositiveAdjustmentGetInvenTransListRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.PositiveAdjustmentGetInvenTransList(
            request.ModuleId,
            nameof(PositiveAdjustment),
            cancellationToken);

        return new PositiveAdjustmentGetInvenTransListResult
        {
            Data = entity
        };
    }
}