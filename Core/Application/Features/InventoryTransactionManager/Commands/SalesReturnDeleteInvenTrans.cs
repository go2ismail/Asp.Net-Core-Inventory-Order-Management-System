using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class SalesReturnDeleteInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class SalesReturnDeleteInvenTransRequest : IRequest<SalesReturnDeleteInvenTransResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }

}

public class SalesReturnDeleteInvenTransValidator : AbstractValidator<SalesReturnDeleteInvenTransRequest>
{
    public SalesReturnDeleteInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeletedById).NotEmpty();
    }
}

public class SalesReturnDeleteInvenTransHandler : IRequestHandler<SalesReturnDeleteInvenTransRequest, SalesReturnDeleteInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public SalesReturnDeleteInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<SalesReturnDeleteInvenTransResult> Handle(SalesReturnDeleteInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.SalesReturnDeleteInvenTrans(
            request.Id,
            request.DeletedById,
            cancellationToken);

        return new SalesReturnDeleteInvenTransResult
        {
            Data = entity
        };
    }
}