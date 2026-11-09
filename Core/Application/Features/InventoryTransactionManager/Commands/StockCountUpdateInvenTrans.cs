using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class StockCountUpdateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class StockCountUpdateInvenTransRequest : IRequest<StockCountUpdateInvenTransResult>
{
    public string? Id { get; init; }
    public string? ProductId { get; init; }
    public double? QtySCCount { get; init; }
    public string? UpdatedById { get; init; }

}

public class StockCountUpdateInvenTransValidator : AbstractValidator<StockCountUpdateInvenTransRequest>
{
    public StockCountUpdateInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.QtySCCount).NotEmpty();
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class StockCountUpdateInvenTransHandler : IRequestHandler<StockCountUpdateInvenTransRequest, StockCountUpdateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public StockCountUpdateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<StockCountUpdateInvenTransResult> Handle(StockCountUpdateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.StockCountUpdateInvenTrans(
            request.Id,
            request.ProductId,
            request.QtySCCount,
            request.UpdatedById,
            cancellationToken);

        return new StockCountUpdateInvenTransResult
        {
            Data = entity
        };
    }
}