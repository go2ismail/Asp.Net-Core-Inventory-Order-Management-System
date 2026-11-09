using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class GoodsReceiveDeleteInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class GoodsReceiveDeleteInvenTransRequest : IRequest<GoodsReceiveDeleteInvenTransResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }

}

public class GoodsReceiveDeleteInvenTransValidator : AbstractValidator<GoodsReceiveDeleteInvenTransRequest>
{
    public GoodsReceiveDeleteInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeletedById).NotEmpty();
    }
}

public class GoodsReceiveDeleteInvenTransHandler : IRequestHandler<GoodsReceiveDeleteInvenTransRequest, GoodsReceiveDeleteInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public GoodsReceiveDeleteInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<GoodsReceiveDeleteInvenTransResult> Handle(GoodsReceiveDeleteInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.GoodsReceiveDeleteInvenTrans(
            request.Id,
            request.DeletedById,
            cancellationToken);

        return new GoodsReceiveDeleteInvenTransResult
        {
            Data = entity
        };
    }
}