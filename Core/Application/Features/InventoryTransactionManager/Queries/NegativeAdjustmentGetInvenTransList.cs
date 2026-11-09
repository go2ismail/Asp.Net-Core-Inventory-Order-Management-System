using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Queries;

public class NegativeAdjustmentGetInvenTransListResult
{
    public List<InventoryTransaction>? Data { get; set; }
}

public class NegativeAdjustmentGetInvenTransListRequest : IRequest<NegativeAdjustmentGetInvenTransListResult>
{
    public string? ModuleId { get; init; }

}

public class NegativeAdjustmentGetInvenTransListValidator : AbstractValidator<NegativeAdjustmentGetInvenTransListRequest>
{
    public NegativeAdjustmentGetInvenTransListValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
    }
}

public class NegativeAdjustmentGetInvenTransListHandler : IRequestHandler<NegativeAdjustmentGetInvenTransListRequest, NegativeAdjustmentGetInvenTransListResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public NegativeAdjustmentGetInvenTransListHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<NegativeAdjustmentGetInvenTransListResult> Handle(NegativeAdjustmentGetInvenTransListRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.NegativeAdjustmentGetInvenTransList(
            request.ModuleId,
            nameof(NegativeAdjustment),
            cancellationToken);

        return new NegativeAdjustmentGetInvenTransListResult
        {
            Data = entity
        };
    }
}