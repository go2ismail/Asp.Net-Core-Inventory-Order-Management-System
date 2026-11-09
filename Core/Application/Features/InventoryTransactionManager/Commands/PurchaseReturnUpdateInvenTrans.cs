using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class PurchaseReturnUpdateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class PurchaseReturnUpdateInvenTransRequest : IRequest<PurchaseReturnUpdateInvenTransResult>
{
    public string? Id { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? UpdatedById { get; init; }

}

public class PurchaseReturnUpdateInvenTransValidator : AbstractValidator<PurchaseReturnUpdateInvenTransRequest>
{
    public PurchaseReturnUpdateInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class PurchaseReturnUpdateInvenTransHandler : IRequestHandler<PurchaseReturnUpdateInvenTransRequest, PurchaseReturnUpdateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public PurchaseReturnUpdateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<PurchaseReturnUpdateInvenTransResult> Handle(PurchaseReturnUpdateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.PurchaseReturnUpdateInvenTrans(
            request.Id,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.UpdatedById,
            cancellationToken);

        return new PurchaseReturnUpdateInvenTransResult
        {
            Data = entity
        };
    }
}