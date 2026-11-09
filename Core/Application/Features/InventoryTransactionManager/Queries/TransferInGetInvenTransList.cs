using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Queries;

public class TransferInGetInvenTransListResult
{
    public List<InventoryTransaction>? Data { get; set; }
}

public class TransferInGetInvenTransListRequest : IRequest<TransferInGetInvenTransListResult>
{
    public string? ModuleId { get; init; }

}

public class TransferInGetInvenTransListValidator : AbstractValidator<TransferInGetInvenTransListRequest>
{
    public TransferInGetInvenTransListValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
    }
}

public class TransferInGetInvenTransListHandler : IRequestHandler<TransferInGetInvenTransListRequest, TransferInGetInvenTransListResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public TransferInGetInvenTransListHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<TransferInGetInvenTransListResult> Handle(TransferInGetInvenTransListRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.TransferInGetInvenTransList(
            request.ModuleId,
            nameof(TransferIn),
            cancellationToken);

        return new TransferInGetInvenTransListResult
        {
            Data = entity
        };
    }
}