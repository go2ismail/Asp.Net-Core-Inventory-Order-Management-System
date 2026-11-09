using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Queries;

public class StockCountGetInvenTransListResult
{
    public List<InventoryTransaction>? Data { get; set; }
}

public class StockCountGetInvenTransListRequest : IRequest<StockCountGetInvenTransListResult>
{
    public string? ModuleId { get; init; }

}

public class StockCountGetInvenTransListValidator : AbstractValidator<StockCountGetInvenTransListRequest>
{
    public StockCountGetInvenTransListValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
    }
}

public class StockCountGetInvenTransListHandler : IRequestHandler<StockCountGetInvenTransListRequest, StockCountGetInvenTransListResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public StockCountGetInvenTransListHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<StockCountGetInvenTransListResult> Handle(StockCountGetInvenTransListRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.StockCountGetInvenTransList(
            request.ModuleId,
            nameof(StockCount),
            cancellationToken);

        return new StockCountGetInvenTransListResult
        {
            Data = entity
        };
    }
}