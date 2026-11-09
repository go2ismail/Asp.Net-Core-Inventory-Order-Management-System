using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class GoodsReceiveUpdateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class GoodsReceiveUpdateInvenTransRequest : IRequest<GoodsReceiveUpdateInvenTransResult>
{
    public string? Id { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? UpdatedById { get; init; }

}

public class GoodsReceiveUpdateInvenTransValidator : AbstractValidator<GoodsReceiveUpdateInvenTransRequest>
{
    public GoodsReceiveUpdateInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class GoodsReceiveUpdateInvenTransHandler : IRequestHandler<GoodsReceiveUpdateInvenTransRequest, GoodsReceiveUpdateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public GoodsReceiveUpdateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<GoodsReceiveUpdateInvenTransResult> Handle(GoodsReceiveUpdateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.GoodsReceiveUpdateInvenTrans(
            request.Id,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.UpdatedById,
            cancellationToken);

        return new GoodsReceiveUpdateInvenTransResult
        {
            Data = entity
        };
    }
}