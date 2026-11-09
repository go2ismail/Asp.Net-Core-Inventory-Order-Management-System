using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class SalesReturnUpdateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class SalesReturnUpdateInvenTransRequest : IRequest<SalesReturnUpdateInvenTransResult>
{
    public string? Id { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? UpdatedById { get; init; }

}

public class SalesReturnUpdateInvenTransValidator : AbstractValidator<SalesReturnUpdateInvenTransRequest>
{
    public SalesReturnUpdateInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class SalesReturnUpdateInvenTransHandler : IRequestHandler<SalesReturnUpdateInvenTransRequest, SalesReturnUpdateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public SalesReturnUpdateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<SalesReturnUpdateInvenTransResult> Handle(SalesReturnUpdateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.SalesReturnUpdateInvenTrans(
            request.Id,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.UpdatedById,
            cancellationToken);

        return new SalesReturnUpdateInvenTransResult
        {
            Data = entity
        };
    }
}