using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerCategoryManager.Commands;

public class DeleteCustomerCategoryResult
{
    public CustomerCategory? Data { get; set; }
}

public class DeleteCustomerCategoryRequest : IRequest<DeleteCustomerCategoryResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteCustomerCategoryValidator : AbstractValidator<DeleteCustomerCategoryRequest>
{
    public DeleteCustomerCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteCustomerCategoryHandler : IRequestHandler<DeleteCustomerCategoryRequest, DeleteCustomerCategoryResult>
{
    private readonly ICommandRepository<CustomerCategory> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerCategoryHandler(
        ICommandRepository<CustomerCategory> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteCustomerCategoryResult> Handle(DeleteCustomerCategoryRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteCustomerCategoryResult
        {
            Data = entity
        };
    }
}

