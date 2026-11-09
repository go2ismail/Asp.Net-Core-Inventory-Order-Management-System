using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class GoodsReceiveCreateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class GoodsReceiveCreateInvenTransRequest : IRequest<GoodsReceiveCreateInvenTransResult>
{
    public string? ModuleId { get; init; }
    public string? WarehouseId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? CreatedById { get; init; }
}

public class GoodsReceiveCreateInvenTransValidator : AbstractValidator<GoodsReceiveCreateInvenTransRequest>
{
    public GoodsReceiveCreateInvenTransValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.CreatedById).NotEmpty();
    }
}

public class GoodsReceiveCreateInvenTransHandler : IRequestHandler<GoodsReceiveCreateInvenTransRequest, GoodsReceiveCreateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public GoodsReceiveCreateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<GoodsReceiveCreateInvenTransResult> Handle(GoodsReceiveCreateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.GoodsReceiveCreateInvenTrans(
            request.ModuleId,
            request.WarehouseId,
            request.ProductId,
            request.Movement,
            request.CreatedById,
            cancellationToken);

        return new GoodsReceiveCreateInvenTransResult
        {
            Data = entity
        };
    }
}