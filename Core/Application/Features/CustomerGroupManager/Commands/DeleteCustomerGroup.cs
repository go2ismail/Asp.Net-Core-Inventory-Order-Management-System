using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerGroupManager.Commands;

public class DeleteCustomerGroupResult
{
    public CustomerGroup? Data { get; set; }
}

public class DeleteCustomerGroupRequest : IRequest<DeleteCustomerGroupResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteCustomerGroupValidator : AbstractValidator<DeleteCustomerGroupRequest>
{
    public DeleteCustomerGroupValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteCustomerGroupHandler : IRequestHandler<DeleteCustomerGroupRequest, DeleteCustomerGroupResult>
{
    private readonly ICommandRepository<CustomerGroup> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerGroupHandler(
        ICommandRepository<CustomerGroup> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteCustomerGroupResult> Handle(DeleteCustomerGroupRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteCustomerGroupResult
        {
            Data = entity
        };
    }
}

