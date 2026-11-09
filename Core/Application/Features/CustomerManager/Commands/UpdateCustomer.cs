using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerManager.Commands;

public class UpdateCustomerResult
{
    public Customer? Data { get; set; }
}

public class UpdateCustomerRequest : IRequest<UpdateCustomerResult>
{
    public string? Id { get; init; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FaxNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? Website { get; set; }
    public string? WhatsApp { get; set; }
    public string? LinkedIn { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? TwitterX { get; set; }
    public string? TikTok { get; set; }
    public string? CustomerGroupId { get; set; }
    public string? CustomerCategoryId { get; set; }
    public string? CreatedById { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.ZipCode).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.EmailAddress).NotEmpty();
        RuleFor(x => x.CustomerGroupId).NotEmpty();
        RuleFor(x => x.CustomerCategoryId).NotEmpty();
    }
}

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResult>
{
    private readonly ICommandRepository<Customer> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerHandler(
        ICommandRepository<Customer> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCustomerResult> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Street = request.Street;
        entity.City = request.City;
        entity.State = request.State;
        entity.ZipCode = request.ZipCode;
        entity.Country = request.Country;
        entity.PhoneNumber = request.PhoneNumber;
        entity.FaxNumber = request.FaxNumber;
        entity.EmailAddress = request.EmailAddress;
        entity.Website = request.Website;
        entity.WhatsApp = request.WhatsApp;
        entity.LinkedIn = request.LinkedIn;
        entity.Facebook = request.Facebook;
        entity.Instagram = request.Instagram;
        entity.TwitterX = request.TwitterX;
        entity.TikTok = request.TikTok;
        entity.CustomerGroupId = request.CustomerGroupId;
        entity.CustomerCategoryId = request.CustomerCategoryId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateCustomerResult
        {
            Data = entity
        };
    }
}

