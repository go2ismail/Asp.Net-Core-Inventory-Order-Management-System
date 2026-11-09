using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerGroupManager.Commands;

public class UpdateCustomerGroupResult
{
    public CustomerGroup? Data { get; set; }
}

public class UpdateCustomerGroupRequest : IRequest<UpdateCustomerGroupResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateCustomerGroupValidator : AbstractValidator<UpdateCustomerGroupRequest>
{
    public UpdateCustomerGroupValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateCustomerGroupHandler : IRequestHandler<UpdateCustomerGroupRequest, UpdateCustomerGroupResult>
{
    private readonly ICommandRepository<CustomerGroup> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerGroupHandler(
        ICommandRepository<CustomerGroup> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCustomerGroupResult> Handle(UpdateCustomerGroupRequest request, CancellationToken cancellationToken)
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

        return new UpdateCustomerGroupResult
        {
            Data = entity
        };
    }
}

