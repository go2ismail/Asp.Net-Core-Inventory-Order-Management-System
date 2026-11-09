using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.VendorGroupManager.Commands;

public class UpdateVendorGroupResult
{
    public VendorGroup? Data { get; set; }
}

public class UpdateVendorGroupRequest : IRequest<UpdateVendorGroupResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateVendorGroupValidator : AbstractValidator<UpdateVendorGroupRequest>
{
    public UpdateVendorGroupValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateVendorGroupHandler : IRequestHandler<UpdateVendorGroupRequest, UpdateVendorGroupResult>
{
    private readonly ICommandRepository<VendorGroup> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVendorGroupHandler(
        ICommandRepository<VendorGroup> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateVendorGroupResult> Handle(UpdateVendorGroupRequest request, CancellationToken cancellationToken)
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

        return new UpdateVendorGroupResult
        {
            Data = entity
        };
    }
}

