using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class StockCountCreateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class StockCountCreateInvenTransRequest : IRequest<StockCountCreateInvenTransResult>
{
    public string? ModuleId { get; init; }
    public string? ProductId { get; init; }
    public double? QtySCCount { get; init; }
    public string? CreatedById { get; init; }
}

public class StockCountCreateInvenTransValidator : AbstractValidator<StockCountCreateInvenTransRequest>
{
    public StockCountCreateInvenTransValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.QtySCCount).NotEmpty();
        RuleFor(x => x.CreatedById).NotEmpty();
    }
}

public class StockCountCreateInvenTransHandler : IRequestHandler<StockCountCreateInvenTransRequest, StockCountCreateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public StockCountCreateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<StockCountCreateInvenTransResult> Handle(StockCountCreateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.StockCountCreateInvenTrans(
            request.ModuleId,
            request.ProductId,
            request.QtySCCount,
            request.CreatedById,
            cancellationToken);

        return new StockCountCreateInvenTransResult
        {
            Data = entity
        };
    }
}