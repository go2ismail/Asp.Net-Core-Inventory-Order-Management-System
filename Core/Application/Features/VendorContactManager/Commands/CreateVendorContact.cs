using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.VendorContactManager.Commands;

public class CreateVendorContactResult
{
    public VendorContact? Data { get; set; }
}

public class CreateVendorContactRequest : IRequest<CreateVendorContactResult>
{
    public string? Name { get; init; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? Description { get; set; }
    public string? VendorId { get; set; }
    public string? CreatedById { get; init; }
}

public class CreateVendorContactValidator : AbstractValidator<CreateVendorContactRequest>
{
    public CreateVendorContactValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.JobTitle).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.EmailAddress).NotEmpty();
    }
}

public class CreateVendorContactHandler : IRequestHandler<CreateVendorContactRequest, CreateVendorContactResult>
{
    private readonly ICommandRepository<VendorContact> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;

    public CreateVendorContactHandler(
        ICommandRepository<VendorContact> repository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
    }

    public async Task<CreateVendorContactResult> Handle(CreateVendorContactRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new VendorContact();
        entity.CreatedById = request.CreatedById;

        entity.Name = request.Name;
        entity.Number = _numberSequenceService.GenerateNumber(nameof(VendorContact), "", "VC");
        entity.JobTitle = request.JobTitle;
        entity.PhoneNumber = request.PhoneNumber;
        entity.EmailAddress = request.EmailAddress;
        entity.Description = request.Description;
        entity.VendorId = request.VendorId;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateVendorContactResult
        {
            Data = entity
        };
    }
}