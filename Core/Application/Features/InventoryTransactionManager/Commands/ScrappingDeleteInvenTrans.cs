using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class ScrappingDeleteInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class ScrappingDeleteInvenTransRequest : IRequest<ScrappingDeleteInvenTransResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }

}

public class ScrappingDeleteInvenTransValidator : AbstractValidator<ScrappingDeleteInvenTransRequest>
{
    public ScrappingDeleteInvenTransValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeletedById).NotEmpty();
    }
}

public class ScrappingDeleteInvenTransHandler : IRequestHandler<ScrappingDeleteInvenTransRequest, ScrappingDeleteInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public ScrappingDeleteInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<ScrappingDeleteInvenTransResult> Handle(ScrappingDeleteInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.ScrappingDeleteInvenTrans(
            request.Id,
            request.DeletedById,
            cancellationToken);

        return new ScrappingDeleteInvenTransResult
        {
            Data = entity
        };
    }
}