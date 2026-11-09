using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Queries;

public class ScrappingGetInvenTransListResult
{
    public List<InventoryTransaction>? Data { get; set; }
}

public class ScrappingGetInvenTransListRequest : IRequest<ScrappingGetInvenTransListResult>
{
    public string? ModuleId { get; init; }

}

public class ScrappingGetInvenTransListValidator : AbstractValidator<ScrappingGetInvenTransListRequest>
{
    public ScrappingGetInvenTransListValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
    }
}

public class ScrappingGetInvenTransListHandler : IRequestHandler<ScrappingGetInvenTransListRequest, ScrappingGetInvenTransListResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public ScrappingGetInvenTransListHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<ScrappingGetInvenTransListResult> Handle(ScrappingGetInvenTransListRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.ScrappingGetInvenTransList(
            request.ModuleId,
            nameof(Scrapping),
            cancellationToken);

        return new ScrappingGetInvenTransListResult
        {
            Data = entity
        };
    }
}