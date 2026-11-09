using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class ScrappingUpdateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class ScrappingUpdateInvenTransRequest : IRequest<ScrappingUpdateInvenTransResult>
{
    public string? Id { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? UpdatedById { get; init; }

}

public class ScrappingUpdateInvenTransValidator : AbstractValidator<ScrappingUpdateInvenTransRequest>
{
    public ScrappingUpdateInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class ScrappingUpdateInvenTransHandler : IRequestHandler<ScrappingUpdateInvenTransRequest, ScrappingUpdateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public ScrappingUpdateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<ScrappingUpdateInvenTransResult> Handle(ScrappingUpdateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.ScrappingUpdateInvenTrans(
            request.Id,
            request.ProductId,
            request.Movement,
            request.UpdatedById,
            cancellationToken);

        return new ScrappingUpdateInvenTransResult
        {
            Data = entity
        };
    }
}