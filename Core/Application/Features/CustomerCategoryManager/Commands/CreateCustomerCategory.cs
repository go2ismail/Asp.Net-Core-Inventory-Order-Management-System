using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerCategoryManager.Commands;

public class CreateCustomerCategoryResult
{
    public CustomerCategory? Data { get; set; }
}

public class CreateCustomerCategoryRequest : IRequest<CreateCustomerCategoryResult>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateCustomerCategoryValidator : AbstractValidator<CreateCustomerCategoryRequest>
{
    public CreateCustomerCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateCustomerCategoryHandler : IRequestHandler<CreateCustomerCategoryRequest, CreateCustomerCategoryResult>
{
    private readonly ICommandRepository<CustomerCategory> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCategoryHandler(
        ICommandRepository<CustomerCategory> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCustomerCategoryResult> Handle(CreateCustomerCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new CustomerCategory();
        entity.CreatedById = request.CreatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateCustomerCategoryResult
        {
            Data = entity
        };
    }
}