using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.GoodsReceiveManager.Commands;

public class UpdateGoodsReceiveResult
{
    public GoodsReceive? Data { get; set; }
}

public class UpdateGoodsReceiveRequest : IRequest<UpdateGoodsReceiveResult>
{
    public string? Id { get; init; }
    public DateTime? ReceiveDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? PurchaseOrderId { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateGoodsReceiveValidator : AbstractValidator<UpdateGoodsReceiveRequest>
{
    public UpdateGoodsReceiveValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ReceiveDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
    }
}

public class UpdateGoodsReceiveHandler : IRequestHandler<UpdateGoodsReceiveRequest, UpdateGoodsReceiveResult>
{
    private readonly ICommandRepository<GoodsReceive> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public UpdateGoodsReceiveHandler(
        ICommandRepository<GoodsReceive> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<UpdateGoodsReceiveResult> Handle(UpdateGoodsReceiveRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.ReceiveDate = request.ReceiveDate;
        entity.Status = (GoodsReceiveStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.PurchaseOrderId = request.PurchaseOrderId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        await _inventoryTransactionService.PropagateParentUpdate(
            entity.Id,
            nameof(GoodsReceive),
            entity.ReceiveDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            null,
            cancellationToken
            );

        return new UpdateGoodsReceiveResult
        {
            Data = entity
        };
    }
}

