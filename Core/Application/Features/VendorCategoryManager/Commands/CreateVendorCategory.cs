using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.VendorCategoryManager.Commands;

public class CreateVendorCategoryResult
{
    public VendorCategory? Data { get; set; }
}

public class CreateVendorCategoryRequest : IRequest<CreateVendorCategoryResult>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateVendorCategoryValidator : AbstractValidator<CreateVendorCategoryRequest>
{
    public CreateVendorCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateVendorCategoryHandler : IRequestHandler<CreateVendorCategoryRequest, CreateVendorCategoryResult>
{
    private readonly ICommandRepository<VendorCategory> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVendorCategoryHandler(
        ICommandRepository<VendorCategory> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateVendorCategoryResult> Handle(CreateVendorCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new VendorCategory();
        entity.CreatedById = request.CreatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateVendorCategoryResult
        {
            Data = entity
        };
    }
}