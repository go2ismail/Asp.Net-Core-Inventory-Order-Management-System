using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class TransferInCreateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class TransferInCreateInvenTransRequest : IRequest<TransferInCreateInvenTransResult>
{
    public string? ModuleId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? CreatedById { get; init; }
}

public class TransferInCreateInvenTransValidator : AbstractValidator<TransferInCreateInvenTransRequest>
{
    public TransferInCreateInvenTransValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.CreatedById).NotEmpty();
    }
}

public class TransferInCreateInvenTransHandler : IRequestHandler<TransferInCreateInvenTransRequest, TransferInCreateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public TransferInCreateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<TransferInCreateInvenTransResult> Handle(TransferInCreateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.TransferInCreateInvenTrans(
            request.ModuleId,
            request.ProductId,
            request.Movement,
            request.CreatedById,
            cancellationToken);

        return new TransferInCreateInvenTransResult
        {
            Data = entity
        };
    }
}