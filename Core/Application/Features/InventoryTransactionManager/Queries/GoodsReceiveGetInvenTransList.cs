using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Queries;

public class GoodsReceiveGetInvenTransListResult
{
    public List<InventoryTransaction>? Data { get; set; }
}

public class GoodsReceiveGetInvenTransListRequest : IRequest<GoodsReceiveGetInvenTransListResult>
{
    public string? ModuleId { get; init; }

}

public class GoodsReceiveGetInvenTransListValidator : AbstractValidator<GoodsReceiveGetInvenTransListRequest>
{
    public GoodsReceiveGetInvenTransListValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
    }
}

public class GoodsReceiveGetInvenTransListHandler : IRequestHandler<GoodsReceiveGetInvenTransListRequest, GoodsReceiveGetInvenTransListResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public GoodsReceiveGetInvenTransListHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<GoodsReceiveGetInvenTransListResult> Handle(GoodsReceiveGetInvenTransListRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.GoodsReceiveGetInvenTransList(
            request.ModuleId,
            nameof(GoodsReceive),
            cancellationToken);

        return new GoodsReceiveGetInvenTransListResult
        {
            Data = entity
        };
    }
}