using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class TransferInUpdateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class TransferInUpdateInvenTransRequest : IRequest<TransferInUpdateInvenTransResult>
{
    public string? Id { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? UpdatedById { get; init; }

}

public class TransferInUpdateInvenTransValidator : AbstractValidator<TransferInUpdateInvenTransRequest>
{
    public TransferInUpdateInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class TransferInUpdateInvenTransHandler : IRequestHandler<TransferInUpdateInvenTransRequest, TransferInUpdateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public TransferInUpdateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<TransferInUpdateInvenTransResult> Handle(TransferInUpdateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.TransferInUpdateInvenTrans(
            request.Id,
            request.ProductId,
            request.Movement,
            request.UpdatedById,
            cancellationToken);

        return new TransferInUpdateInvenTransResult
        {
            Data = entity
        };
    }
}