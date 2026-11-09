using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.TransferOutManager.Commands;

public class CreateTransferOutResult
{
    public TransferOut? Data { get; set; }
}

public class CreateTransferOutRequest : IRequest<CreateTransferOutResult>
{
    public DateTime? TransferReleaseDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? WarehouseFromId { get; init; }
    public string? WarehouseToId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateTransferOutValidator : AbstractValidator<CreateTransferOutRequest>
{
    public CreateTransferOutValidator()
    {
        RuleFor(x => x.TransferReleaseDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.WarehouseFromId).NotEmpty();
        RuleFor(x => x.WarehouseToId).NotEmpty();
    }
}

public class CreateTransferOutHandler : IRequestHandler<CreateTransferOutRequest, CreateTransferOutResult>
{
    private readonly ICommandRepository<TransferOut> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;

    public CreateTransferOutHandler(
        ICommandRepository<TransferOut> repository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
    }

    public async Task<CreateTransferOutResult> Handle(CreateTransferOutRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new TransferOut();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(TransferOut), "", "OUT");
        entity.TransferReleaseDate = request.TransferReleaseDate;
        entity.Status = (TransferStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.WarehouseFromId = request.WarehouseFromId;
        entity.WarehouseToId = request.WarehouseToId;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateTransferOutResult
        {
            Data = entity
        };
    }
}