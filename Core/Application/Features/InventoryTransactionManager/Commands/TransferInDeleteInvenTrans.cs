using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class TransferInDeleteInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class TransferInDeleteInvenTransRequest : IRequest<TransferInDeleteInvenTransResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }

}

public class TransferInDeleteInvenTransValidator : AbstractValidator<TransferInDeleteInvenTransRequest>
{
    public TransferInDeleteInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeletedById).NotEmpty();
    }
}

public class TransferInDeleteInvenTransHandler : IRequestHandler<TransferInDeleteInvenTransRequest, TransferInDeleteInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public TransferInDeleteInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<TransferInDeleteInvenTransResult> Handle(TransferInDeleteInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.TransferInDeleteInvenTrans(
            request.Id,
            request.DeletedById,
            cancellationToken);

        return new TransferInDeleteInvenTransResult
        {
            Data = entity
        };
    }
}