using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.ProductGroupManager.Commands;

public class CreateProductGroupResult
{
    public ProductGroup? Data { get; set; }
}

public class CreateProductGroupRequest : IRequest<CreateProductGroupResult>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateProductGroupValidator : AbstractValidator<CreateProductGroupRequest>
{
    public CreateProductGroupValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateProductGroupHandler : IRequestHandler<CreateProductGroupRequest, CreateProductGroupResult>
{
    private readonly ICommandRepository<ProductGroup> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductGroupHandler(
        ICommandRepository<ProductGroup> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateProductGroupResult> Handle(CreateProductGroupRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new ProductGroup();
        entity.CreatedById = request.CreatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateProductGroupResult
        {
            Data = entity
        };
    }
}