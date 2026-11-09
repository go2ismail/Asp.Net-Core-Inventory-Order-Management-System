using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TaxManager.Commands;

public class DeleteTaxResult
{
    public Tax? Data { get; set; }
}

public class DeleteTaxRequest : IRequest<DeleteTaxResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteTaxValidator : AbstractValidator<DeleteTaxRequest>
{
    public DeleteTaxValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteTaxHandler : IRequestHandler<DeleteTaxRequest, DeleteTaxResult>
{
    private readonly ICommandRepository<Tax> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaxHandler(
        ICommandRepository<Tax> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteTaxResult> Handle(DeleteTaxRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteTaxResult
        {
            Data = entity
        };
    }
}

