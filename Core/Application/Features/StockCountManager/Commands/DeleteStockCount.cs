using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.StockCountManager.Commands;

public class DeleteStockCountResult
{
    public StockCount? Data { get; set; }
}

public class DeleteStockCountRequest : IRequest<DeleteStockCountResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteStockCountValidator : AbstractValidator<DeleteStockCountRequest>
{
    public DeleteStockCountValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteStockCountHandler : IRequestHandler<DeleteStockCountRequest, DeleteStockCountResult>
{
    private readonly ICommandRepository<StockCount> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public DeleteStockCountHandler(
        ICommandRepository<StockCount> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<DeleteStockCountResult> Handle(DeleteStockCountRequest request, CancellationToken cancellationToken)
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
            nameof(StockCount),
            entity.CountDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            null,
            cancellationToken
            );

        return new DeleteStockCountResult
        {
            Data = entity
        };
    }
}

