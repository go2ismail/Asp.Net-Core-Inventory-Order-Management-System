using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.UnitMeasureManager.Commands;

public class DeleteUnitMeasureResult
{
    public UnitMeasure? Data { get; set; }
}

public class DeleteUnitMeasureRequest : IRequest<DeleteUnitMeasureResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteUnitMeasureValidator : AbstractValidator<DeleteUnitMeasureRequest>
{
    public DeleteUnitMeasureValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteUnitMeasureHandler : IRequestHandler<DeleteUnitMeasureRequest, DeleteUnitMeasureResult>
{
    private readonly ICommandRepository<UnitMeasure> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUnitMeasureHandler(
        ICommandRepository<UnitMeasure> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteUnitMeasureResult> Handle(DeleteUnitMeasureRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteUnitMeasureResult
        {
            Data = entity
        };
    }
}

