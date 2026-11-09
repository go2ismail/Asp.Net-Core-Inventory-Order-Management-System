using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.SalesReturnManager.Commands;

public class DeleteSalesReturnResult
{
    public SalesReturn? Data { get; set; }
}

public class DeleteSalesReturnRequest : IRequest<DeleteSalesReturnResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteSalesReturnValidator : AbstractValidator<DeleteSalesReturnRequest>
{
    public DeleteSalesReturnValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteSalesReturnHandler : IRequestHandler<DeleteSalesReturnRequest, DeleteSalesReturnResult>
{
    private readonly ICommandRepository<SalesReturn> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeleteSalesReturnHandler(
        ICommandRepository<SalesReturn> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeleteSalesReturnResult> Handle(DeleteSalesReturnRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        await _inventoryTransactionService.PropagateParentUpdate(
            entity.Id,
            nameof(SalesReturn),
            entity.ReturnDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            null,
            cancellationToken
            );

        return new DeleteSalesReturnResult
        {
            Data = entity
        };
    }
}

