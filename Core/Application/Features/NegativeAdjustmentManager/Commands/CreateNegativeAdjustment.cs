using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.NegativeAdjustmentManager.Commands;

public class CreateNegativeAdjustmentResult
{
    public NegativeAdjustment? Data { get; set; }
}

public class CreateNegativeAdjustmentRequest : IRequest<CreateNegativeAdjustmentResult>
{
    public string? Number { get; init; }
    public DateTime? AdjustmentDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateNegativeAdjustmentValidator : AbstractValidator<CreateNegativeAdjustmentRequest>
{
    public CreateNegativeAdjustmentValidator()
    {
        RuleFor(x => x.AdjustmentDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}

public class CreateNegativeAdjustmentHandler : IRequestHandler<CreateNegativeAdjustmentRequest, CreateNegativeAdjustmentResult>
{
    private readonly ICommandRepository<NegativeAdjustment> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;

    public CreateNegativeAdjustmentHandler(
        ICommandRepository<NegativeAdjustment> repository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
    }

    public async Task<CreateNegativeAdjustmentResult> Handle(CreateNegativeAdjustmentRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new NegativeAdjustment();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(NegativeAdjustment), "", "ADJ-");
        entity.AdjustmentDate = request.AdjustmentDate;
        entity.Status = (AdjustmentStatus)int.Parse(request.Status!);
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateNegativeAdjustmentResult
        {
            Data = entity
        };
    }
}