using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class TransferOutDeleteInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class TransferOutDeleteInvenTransRequest : IRequest<TransferOutDeleteInvenTransResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }

}

public class TransferOutDeleteInvenTransValidator : AbstractValidator<TransferOutDeleteInvenTransRequest>
{
    public TransferOutDeleteInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeletedById).NotEmpty();
    }
}

public class TransferOutDeleteInvenTransHandler : IRequestHandler<TransferOutDeleteInvenTransRequest, TransferOutDeleteInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public TransferOutDeleteInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<TransferOutDeleteInvenTransResult> Handle(TransferOutDeleteInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.TransferOutDeleteInvenTrans(
            request.Id,
            request.DeletedById,
            cancellationToken);

        return new TransferOutDeleteInvenTransResult
        {
            Data = entity
        };
    }
}