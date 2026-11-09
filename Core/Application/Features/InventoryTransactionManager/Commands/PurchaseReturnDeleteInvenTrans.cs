using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class PurchaseReturnDeleteInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class PurchaseReturnDeleteInvenTransRequest : IRequest<PurchaseReturnDeleteInvenTransResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }

}

public class PurchaseReturnDeleteInvenTransValidator : AbstractValidator<PurchaseReturnDeleteInvenTransRequest>
{
    public PurchaseReturnDeleteInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeletedById).NotEmpty();
    }
}

public class PurchaseReturnDeleteInvenTransHandler : IRequestHandler<PurchaseReturnDeleteInvenTransRequest, PurchaseReturnDeleteInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public PurchaseReturnDeleteInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<PurchaseReturnDeleteInvenTransResult> Handle(PurchaseReturnDeleteInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.PurchaseReturnDeleteInvenTrans(
            request.Id,
            request.DeletedById,
            cancellationToken);

        return new PurchaseReturnDeleteInvenTransResult
        {
            Data = entity
        };
    }
}