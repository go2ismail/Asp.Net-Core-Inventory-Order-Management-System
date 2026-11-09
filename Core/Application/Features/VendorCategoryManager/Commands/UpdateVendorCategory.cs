using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.VendorCategoryManager.Commands;

public class UpdateVendorCategoryResult
{
    public VendorCategory? Data { get; set; }
}

public class UpdateVendorCategoryRequest : IRequest<UpdateVendorCategoryResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateVendorCategoryValidator : AbstractValidator<UpdateVendorCategoryRequest>
{
    public UpdateVendorCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateVendorCategoryHandler : IRequestHandler<UpdateVendorCategoryRequest, UpdateVendorCategoryResult>
{
    private readonly ICommandRepository<VendorCategory> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVendorCategoryHandler(
        ICommandRepository<VendorCategory> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateVendorCategoryResult> Handle(UpdateVendorCategoryRequest request, CancellationToken cancellationToken)
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

        return new UpdateVendorCategoryResult
        {
            Data = entity
        };
    }
}

