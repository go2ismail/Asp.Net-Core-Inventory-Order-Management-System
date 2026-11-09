using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerGroupManager.Commands;

public class CreateCustomerGroupResult
{
    public CustomerGroup? Data { get; set; }
}

public class CreateCustomerGroupRequest : IRequest<CreateCustomerGroupResult>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateCustomerGroupValidator : AbstractValidator<CreateCustomerGroupRequest>
{
    public CreateCustomerGroupValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateCustomerGroupHandler : IRequestHandler<CreateCustomerGroupRequest, CreateCustomerGroupResult>
{
    private readonly ICommandRepository<CustomerGroup> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerGroupHandler(
        ICommandRepository<CustomerGroup> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCustomerGroupResult> Handle(CreateCustomerGroupRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new CustomerGroup();
        entity.CreatedById = request.CreatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateCustomerGroupResult
        {
            Data = entity
        };
    }
}