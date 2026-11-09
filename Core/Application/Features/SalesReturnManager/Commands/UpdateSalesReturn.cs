using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.SalesReturnManager.Commands;

public class UpdateSalesReturnResult
{
    public SalesReturn? Data { get; set; }
}

public class UpdateSalesReturnRequest : IRequest<UpdateSalesReturnResult>
{
    public string? Id { get; init; }
    public DateTime? ReturnDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? DeliveryOrderId { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateSalesReturnValidator : AbstractValidator<UpdateSalesReturnRequest>
{
    public UpdateSalesReturnValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ReturnDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.DeliveryOrderId).NotEmpty();
    }
}

public class UpdateSalesReturnHandler : IRequestHandler<UpdateSalesReturnRequest, UpdateSalesReturnResult>
{
    private readonly ICommandRepository<SalesReturn> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public UpdateSalesReturnHandler(
        ICommandRepository<SalesReturn> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<UpdateSalesReturnResult> Handle(UpdateSalesReturnRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.ReturnDate = request.ReturnDate;
        entity.Status = (SalesReturnStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.DeliveryOrderId = request.DeliveryOrderId;

        _repository.Update(entity);
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

        return new UpdateSalesReturnResult
        {
            Data = entity
        };
    }
}

