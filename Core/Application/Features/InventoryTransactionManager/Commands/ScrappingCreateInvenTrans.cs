using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.InventoryTransactionManager.Commands;

public class ScrappingCreateInvenTransResult
{
    public InventoryTransaction? Data { get; set; }
}

public class ScrappingCreateInvenTransRequest : IRequest<ScrappingCreateInvenTransResult>
{
    public string? ModuleId { get; init; }
    public string? ProductId { get; init; }
    public double? Movement { get; init; }
    public string? CreatedById { get; init; }
}

public class ScrappingCreateInvenTransValidator : AbstractValidator<ScrappingCreateInvenTransRequest>
{
    public ScrappingCreateInvenTransValidator()
    {
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Movement).NotEmpty();
        RuleFor(x => x.CreatedById).NotEmpty();
    }
}

public class ScrappingCreateInvenTransHandler : IRequestHandler<ScrappingCreateInvenTransRequest, ScrappingCreateInvenTransResult>
{
    private readonly InventoryTransactionService _inventoryTransactionService;

    public ScrappingCreateInvenTransHandler(
        InventoryTransactionService inventoryTransactionService
        )
    {
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<ScrappingCreateInvenTransResult> Handle(ScrappingCreateInvenTransRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _inventoryTransactionService.ScrappingCreateInvenTrans(
            request.ModuleId,
            request.ProductId,
            request.Movement,
            request.CreatedById,
            cancellationToken);

        return new ScrappingCreateInvenTransResult
        {
            Data = entity
        };
    }
}