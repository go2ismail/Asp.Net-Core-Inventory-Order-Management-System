using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.PositiveAdjustmentManager.Commands;

public class CreatePositiveAdjustmentResult
{
    public PositiveAdjustment? Data { get; set; }
}

public class CreatePositiveAdjustmentRequest : IRequest<CreatePositiveAdjustmentResult>
{
    public DateTime? AdjustmentDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreatePositiveAdjustmentValidator : AbstractValidator<CreatePositiveAdjustmentRequest>
{
    public CreatePositiveAdjustmentValidator()
    {
        RuleFor(x => x.AdjustmentDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}

public class CreatePositiveAdjustmentHandler : IRequestHandler<CreatePositiveAdjustmentRequest, CreatePositiveAdjustmentResult>
{
    private readonly ICommandRepository<PositiveAdjustment> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;

    public CreatePositiveAdjustmentHandler(
        ICommandRepository<PositiveAdjustment> repository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
    }

    public async Task<CreatePositiveAdjustmentResult> Handle(CreatePositiveAdjustmentRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new PositiveAdjustment();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(PositiveAdjustment), "", "ADJ+");
        entity.AdjustmentDate = request.AdjustmentDate;
        entity.Status = (AdjustmentStatus)int.Parse(request.Status!);
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreatePositiveAdjustmentResult
        {
            Data = entity
        };
    }
}