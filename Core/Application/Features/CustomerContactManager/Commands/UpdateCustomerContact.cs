using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerContactManager.Commands;

public class UpdateCustomerContactResult
{
    public CustomerContact? Data { get; set; }
}

public class UpdateCustomerContactRequest : IRequest<UpdateCustomerContactResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? Description { get; set; }
    public string? CustomerId { get; set; }
    public string? UpdatedById { get; init; }
}

public class UpdateCustomerContactValidator : AbstractValidator<UpdateCustomerContactRequest>
{
    public UpdateCustomerContactValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.JobTitle).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.EmailAddress).NotEmpty();
    }
}

public class UpdateCustomerContactHandler : IRequestHandler<UpdateCustomerContactRequest, UpdateCustomerContactResult>
{
    private readonly ICommandRepository<CustomerContact> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerContactHandler(
        ICommandRepository<CustomerContact> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCustomerContactResult> Handle(UpdateCustomerContactRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.Name = request.Name;
        entity.JobTitle = request.JobTitle;
        entity.PhoneNumber = request.PhoneNumber;
        entity.EmailAddress = request.EmailAddress;
        entity.Description = request.Description;
        entity.CustomerId = request.CustomerId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateCustomerContactResult
        {
            Data = entity
        };
    }
}

