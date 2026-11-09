using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.StockCountManager.Commands;

public class CreateStockCountResult
{
    public StockCount? Data { get; set; }
}

public class CreateStockCountRequest : IRequest<CreateStockCountResult>
{
    public DateTime? CountDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? WarehouseId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateStockCountValidator : AbstractValidator<CreateStockCountRequest>
{
    public CreateStockCountValidator()
    {
        RuleFor(x => x.CountDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
    }
}

public class CreateStockCountHandler : IRequestHandler<CreateStockCountRequest, CreateStockCountResult>
{
    private readonly ICommandRepository<StockCount> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;

    public CreateStockCountHandler(
        ICommandRepository<StockCount> repository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
    }

    public async Task<CreateStockCountResult> Handle(CreateStockCountRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new StockCount();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(StockCount), "", "SC");
        entity.CountDate = request.CountDate;
        entity.Status = (StockCountStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.WarehouseId = request.WarehouseId;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateStockCountResult
        {
            Data = entity
        };
    }
}