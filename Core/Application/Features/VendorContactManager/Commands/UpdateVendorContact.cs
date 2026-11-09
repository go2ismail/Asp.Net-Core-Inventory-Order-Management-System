using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.VendorContactManager.Commands;

public class UpdateVendorContactResult
{
    public VendorContact? Data { get; set; }
}

public class UpdateVendorContactRequest : IRequest<UpdateVendorContactResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? Description { get; set; }
    public string? VendorId { get; set; }
    public string? UpdatedById { get; init; }
}

public class UpdateVendorContactValidator : AbstractValidator<UpdateVendorContactRequest>
{
    public UpdateVendorContactValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.JobTitle).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.EmailAddress).NotEmpty();
    }
}

public class UpdateVendorContactHandler : IRequestHandler<UpdateVendorContactRequest, UpdateVendorContactResult>
{
    private readonly ICommandRepository<VendorContact> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVendorContactHandler(
        ICommandRepository<VendorContact> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateVendorContactResult> Handle(UpdateVendorContactRequest request, CancellationToken cancellationToken)
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
        entity.VendorId = request.VendorId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateVendorContactResult
        {
            Data = entity
        };
    }
}

