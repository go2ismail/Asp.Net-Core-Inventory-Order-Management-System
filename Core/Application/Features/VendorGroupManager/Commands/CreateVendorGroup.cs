using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.VendorGroupManager.Commands;

public class CreateVendorGroupResult
{
    public VendorGroup? Data { get; set; }
}

public class CreateVendorGroupRequest : IRequest<CreateVendorGroupResult>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateVendorGroupValidator : AbstractValidator<CreateVendorGroupRequest>
{
    public CreateVendorGroupValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateVendorGroupHandler : IRequestHandler<CreateVendorGroupRequest, CreateVendorGroupResult>
{
    private readonly ICommandRepository<VendorGroup> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVendorGroupHandler(
        ICommandRepository<VendorGroup> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateVendorGroupResult> Handle(CreateVendorGroupRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new VendorGroup();
        entity.CreatedById = request.CreatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateVendorGroupResult
        {
            Data = entity
        };
    }
}