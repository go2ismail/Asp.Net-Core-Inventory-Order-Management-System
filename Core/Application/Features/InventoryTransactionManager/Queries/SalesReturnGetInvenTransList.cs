using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Queries;

public class SalesReturnGetInvenTransListResult
{
    public List<InventoryTransaction>? Data { get; set; }
}

public class SalesReturnGetInvenTransListRequest : IRequest<SalesReturnGetInvenTransListResult>
{
    public string? ModuleId { get; init; }

}

public class SalesReturnGetInvenTransListValidator : AbstractValidator<SalesReturnGetInvenTransListRequest>
{
    public SalesReturnGetInvenTransListValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
    }
}

public class SalesReturnGetInvenTransListHandler : IRequestHandler<SalesReturnGetInvenTransListRequest, SalesReturnGetInvenTransListResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public SalesReturnGetInvenTransListHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<SalesReturnGetInvenTransListResult> Handle(SalesReturnGetInvenTransListRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.SalesReturnGetInvenTransList(
            request.ModuleId,
            nameof(SalesReturn),
            cancellationToken);

        return new SalesReturnGetInvenTransListResult
        {
            Data = entity
        };
    }
}