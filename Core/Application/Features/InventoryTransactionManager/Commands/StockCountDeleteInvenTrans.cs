using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class StockCountDeleteInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class StockCountDeleteInvenTransRequest : IRequest<StockCountDeleteInvenTransResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }

}

public class StockCountDeleteInvenTransValidator : AbstractValidator<StockCountDeleteInvenTransRequest>
{
    public StockCountDeleteInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeletedById).NotEmpty();
    }
}

public class StockCountDeleteInvenTransHandler : IRequestHandler<StockCountDeleteInvenTransRequest, StockCountDeleteInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public StockCountDeleteInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<StockCountDeleteInvenTransResult> Handle(StockCountDeleteInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.StockCountDeleteInvenTrans(
            request.Id,
            request.DeletedById,
            cancellationToken);

        return new StockCountDeleteInvenTransResult
        {
            Data = entity
        };
    }
}