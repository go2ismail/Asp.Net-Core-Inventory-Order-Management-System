using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class SalesReturnCreateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class SalesReturnCreateInvenTransRequest : IRequest<SalesReturnCreateInvenTransResult>
{
    public string? ModuleId { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? CreatedById { get; init; }
}

public class SalesReturnCreateInvenTransValidator : AbstractValidator<SalesReturnCreateInvenTransRequest>
{
    public SalesReturnCreateInvenTransValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.CreatedById).NotEmpty();
    }
}

public class SalesReturnCreateInvenTransHandler : IRequestHandler<SalesReturnCreateInvenTransRequest, SalesReturnCreateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public SalesReturnCreateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<SalesReturnCreateInvenTransResult> Handle(SalesReturnCreateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.SalesReturnCreateInvenTrans(
            request.ModuleId,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.CreatedById,
            cancellationToken);

        return new SalesReturnCreateInvenTransResult
        {
            Data = entity
        };
    }
}