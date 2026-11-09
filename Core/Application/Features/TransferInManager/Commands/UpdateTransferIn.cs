using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.TransferInManager.Commands;

public class UpdateTransferInResult
{
    public TransferIn? Data { get; set; }
}

public class UpdateTransferInRequest : IRequest<UpdateTransferInResult>
{
    public string? Id { get; init; }
    public DateTime? TransferReceiveDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? TransferOutId { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateTransferInValidator : AbstractValidator<UpdateTransferInRequest>
{
    public UpdateTransferInValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TransferReceiveDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.TransferOutId).NotEmpty();
    }
}

public class UpdateTransferInHandler : IRequestHandler<UpdateTransferInRequest, UpdateTransferInResult>
{
    private readonly ICommandRepository<TransferIn> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;
    private readonly ICommandRepository<TransferOut> _transferOutRepository;

    public UpdateTransferInHandler(
        ICommandRepository<TransferIn> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService,
        ICommandRepository<TransferOut> transferOutRepository
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
        _transferOutRepository = transferOutRepository;
    }

    public async Task<UpdateTransferInResult> Handle(UpdateTransferInRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.TransferReceiveDate = request.TransferReceiveDate;
        entity.Status = (TransferStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.TransferOutId = request.TransferOutId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        var transferOut = await _transferOutRepository.GetAsync(entity.TransferOutId ?? string.Empty, cancellationToken);

        await _inventoryTransactionService.PropagateParentUpdate(
            entity.Id,
            nameof(TransferIn),
            entity.TransferReceiveDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            transferOut?.WarehouseToId,
            cancellationToken
            );

        return new UpdateTransferInResult
        {
            Data = entity
        };
    }
}

