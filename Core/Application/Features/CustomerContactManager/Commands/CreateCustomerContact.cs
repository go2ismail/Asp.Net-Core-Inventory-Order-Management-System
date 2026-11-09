using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerContactManager.Commands;

public class CreateCustomerContactResult
{
    public CustomerContact? Data { get; set; }
}

public class CreateCustomerContactRequest : IRequest<CreateCustomerContactResult>
{
    public string? Name { get; init; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? Description { get; set; }
    public string? CustomerId { get; set; }
    public string? CreatedById { get; init; }
}

public class CreateCustomerContactValidator : AbstractValidator<CreateCustomerContactRequest>
{
    public CreateCustomerContactValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.JobTitle).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.EmailAddress).NotEmpty();
    }
}

public class CreateCustomerContactHandler : IRequestHandler<CreateCustomerContactRequest, CreateCustomerContactResult>
{
    private readonly ICommandRepository<CustomerContact> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;

    public CreateCustomerContactHandler(
        ICommandRepository<CustomerContact> repository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
    }

    public async Task<CreateCustomerContactResult> Handle(CreateCustomerContactRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new CustomerContact();
        entity.CreatedById = request.CreatedById;

        entity.Name = request.Name;
        entity.Number = _numberSequenceService.GenerateNumber(nameof(CustomerContact), "", "CC");
        entity.JobTitle = request.JobTitle;
        entity.PhoneNumber = request.PhoneNumber;
        entity.EmailAddress = request.EmailAddress;
        entity.Description = request.Description;
        entity.CustomerId = request.CustomerId;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateCustomerContactResult
        {
            Data = entity
        };
    }
}