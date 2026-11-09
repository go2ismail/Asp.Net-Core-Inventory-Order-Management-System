using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.ScrappingManager.Commands;

public class CreateScrappingResult
{
    public Scrapping? Data { get; set; }
}

public class CreateScrappingRequest : IRequest<CreateScrappingResult>
{
    public DateTime? ScrappingDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? WarehouseId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateScrappingValidator : AbstractValidator<CreateScrappingRequest>
{
    public CreateScrappingValidator()
    {
        RuleFor(x => x.ScrappingDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
    }
}

public class CreateScrappingHandler : IRequestHandler<CreateScrappingRequest, CreateScrappingResult>
{
    private readonly ICommandRepository<Scrapping> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;

    public CreateScrappingHandler(
        ICommandRepository<Scrapping> repository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
    }

    public async Task<CreateScrappingResult> Handle(CreateScrappingRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Scrapping();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(Scrapping), "", "SCRP");
        entity.ScrappingDate = request.ScrappingDate;
        entity.Status = (ScrappingStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.WarehouseId = request.WarehouseId;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateScrappingResult
        {
            Data = entity
        };
    }
}