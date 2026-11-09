using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerCategoryManager.Commands;

public class UpdateCustomerCategoryResult
{
    public CustomerCategory? Data { get; set; }
}

public class UpdateCustomerCategoryRequest : IRequest<UpdateCustomerCategoryResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateCustomerCategoryValidator : AbstractValidator<UpdateCustomerCategoryRequest>
{
    public UpdateCustomerCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateCustomerCategoryHandler : IRequestHandler<UpdateCustomerCategoryRequest, UpdateCustomerCategoryResult>
{
    private readonly ICommandRepository<CustomerCategory> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerCategoryHandler(
        ICommandRepository<CustomerCategory> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCustomerCategoryResult> Handle(UpdateCustomerCategoryRequest request, CancellationToken cancellationToken)
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

        return new UpdateCustomerCategoryResult
        {
            Data = entity
        };
    }
}

