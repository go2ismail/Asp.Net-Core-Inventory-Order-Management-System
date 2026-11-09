using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TaxManager.Commands;

public class UpdateTaxResult
{
    public Tax? Data { get; set; }
}

public class UpdateTaxRequest : IRequest<UpdateTaxResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public double? Percentage { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateTaxValidator : AbstractValidator<UpdateTaxRequest>
{
    public UpdateTaxValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Percentage).NotEmpty();
    }
}

public class UpdateTaxHandler : IRequestHandler<UpdateTaxRequest, UpdateTaxResult>
{
    private readonly ICommandRepository<Tax> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaxHandler(
        ICommandRepository<Tax> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateTaxResult> Handle(UpdateTaxRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.Name = request.Name;
        entity.Percentage = request.Percentage;
        entity.Description = request.Description;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateTaxResult
        {
            Data = entity
        };
    }
}

