using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.UnitMeasureManager.Commands;

public class UpdateUnitMeasureResult
{
    public UnitMeasure? Data { get; set; }
}

public class UpdateUnitMeasureRequest : IRequest<UpdateUnitMeasureResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateUnitMeasureValidator : AbstractValidator<UpdateUnitMeasureRequest>
{
    public UpdateUnitMeasureValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateUnitMeasureHandler : IRequestHandler<UpdateUnitMeasureRequest, UpdateUnitMeasureResult>
{
    private readonly ICommandRepository<UnitMeasure> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUnitMeasureHandler(
        ICommandRepository<UnitMeasure> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateUnitMeasureResult> Handle(UpdateUnitMeasureRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateUnitMeasureResult
        {
            Data = entity
        };
    }
}

