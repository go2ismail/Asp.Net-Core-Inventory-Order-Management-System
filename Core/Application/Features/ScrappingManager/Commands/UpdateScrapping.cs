using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.ScrappingManager.Commands;

public class UpdateScrappingResult
{
    public Scrapping? Data { get; set; }
}

public class UpdateScrappingRequest : IRequest<UpdateScrappingResult>
{
    public string? Id { get; init; }
    public DateTime? ScrappingDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? WarehouseId { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateScrappingValidator : AbstractValidator<UpdateScrappingRequest>
{
    public UpdateScrappingValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ScrappingDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
    }
}

public class UpdateScrappingHandler : IRequestHandler<UpdateScrappingRequest, UpdateScrappingResult>
{
    private readonly ICommandRepository<Scrapping> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public UpdateScrappingHandler(
        ICommandRepository<Scrapping> repository,
        IUnitOfWork unitOfWork,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<UpdateScrappingResult> Handle(UpdateScrappingRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.ScrappingDate = request.ScrappingDate;
        entity.Status = (ScrappingStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.WarehouseId = request.WarehouseId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        await _inventoryTransactionService.PropagateParentUpdate(
            entity.Id,
            nameof(Scrapping),
            entity.ScrappingDate,
            (InventoryTransactionStatus?)entity.Status,
            entity.IsDeleted,
            entity.UpdatedById,
            entity.WarehouseId,
            cancellationToken
            );

        return new UpdateScrappingResult
        {
            Data = entity
        };
    }
}

