using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class TransferOutUpdateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class TransferOutUpdateInvenTransRequest : IRequest<TransferOutUpdateInvenTransResult>
{
    public string? Id { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? UpdatedById { get; init; }

}

public class TransferOutUpdateInvenTransValidator : AbstractValidator<TransferOutUpdateInvenTransRequest>
{
    public TransferOutUpdateInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class TransferOutUpdateInvenTransHandler : IRequestHandler<TransferOutUpdateInvenTransRequest, TransferOutUpdateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public TransferOutUpdateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<TransferOutUpdateInvenTransResult> Handle(TransferOutUpdateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.TransferOutUpdateInvenTrans(
            request.Id,
            request.ProductId,
            request.Movement,
            request.UpdatedById,
            cancellationToken);

        return new TransferOutUpdateInvenTransResult
        {
            Data = entity
        };
    }
}