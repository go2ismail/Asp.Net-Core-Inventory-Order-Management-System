using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class PurchaseReturnCreateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class PurchaseReturnCreateInvenTransRequest : IRequest<PurchaseReturnCreateInvenTransResult>
{
    public string? ModuleId { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? CreatedById { get; init; }
}

public class PurchaseReturnCreateInvenTransValidator : AbstractValidator<PurchaseReturnCreateInvenTransRequest>
{
    public PurchaseReturnCreateInvenTransValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.CreatedById).NotEmpty();
    }
}

public class PurchaseReturnCreateInvenTransHandler : IRequestHandler<PurchaseReturnCreateInvenTransRequest, PurchaseReturnCreateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public PurchaseReturnCreateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<PurchaseReturnCreateInvenTransResult> Handle(PurchaseReturnCreateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.PurchaseReturnCreateInvenTrans(
            request.ModuleId,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.CreatedById,
            cancellationToken);

        return new PurchaseReturnCreateInvenTransResult
        {
            Data = entity
        };
    }
}