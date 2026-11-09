using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TaxManager.Commands;

public class CreateTaxResult
{
    public Tax? Data { get; set; }
}

public class CreateTaxRequest : IRequest<CreateTaxResult>
{
    public string? Name { get; init; }
    public double? Percentage { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateTaxValidator : AbstractValidator<CreateTaxRequest>
{
    public CreateTaxValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Percentage).NotEmpty();
    }
}

public class CreateTaxHandler : IRequestHandler<CreateTaxRequest, CreateTaxResult>
{
    private readonly ICommandRepository<Tax> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaxHandler(
        ICommandRepository<Tax> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTaxResult> Handle(CreateTaxRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Tax();
        entity.CreatedById = request.CreatedById;

        entity.Name = request.Name;
        entity.Percentage = request.Percentage;
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateTaxResult
        {
            Data = entity
        };
    }
}