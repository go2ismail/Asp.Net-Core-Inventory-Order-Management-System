using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Queries;

public class TransferOutGetInvenTransListResult
{
    public List<InventoryTransaction>? Data { get; set; }
}

public class TransferOutGetInvenTransListRequest : IRequest<TransferOutGetInvenTransListResult>
{
    public string? ModuleId { get; init; }

}

public class TransferOutGetInvenTransListValidator : AbstractValidator<TransferOutGetInvenTransListRequest>
{
    public TransferOutGetInvenTransListValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
    }
}

public class TransferOutGetInvenTransListHandler : IRequestHandler<TransferOutGetInvenTransListRequest, TransferOutGetInvenTransListResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public TransferOutGetInvenTransListHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<TransferOutGetInvenTransListResult> Handle(TransferOutGetInvenTransListRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.TransferOutGetInvenTransList(
            request.ModuleId,
            nameof(TransferOut),
            cancellationToken);

        return new TransferOutGetInvenTransListResult
        {
            Data = entity
        };
    }
}