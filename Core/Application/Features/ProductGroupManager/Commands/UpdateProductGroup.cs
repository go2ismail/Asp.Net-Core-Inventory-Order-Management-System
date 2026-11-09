using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.ProductGroupManager.Commands;

public class UpdateProductGroupResult
{
    public ProductGroup? Data { get; set; }
}

public class UpdateProductGroupRequest : IRequest<UpdateProductGroupResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateProductGroupValidator : AbstractValidator<UpdateProductGroupRequest>
{
    public UpdateProductGroupValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateProductGroupHandler : IRequestHandler<UpdateProductGroupRequest, UpdateProductGroupResult>
{
    private readonly ICommandRepository<ProductGroup> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductGroupHandler(
        ICommandRepository<ProductGroup> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateProductGroupResult> Handle(UpdateProductGroupRequest request, CancellationToken cancellationToken)
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

        return new UpdateProductGroupResult
        {
            Data = entity
        };
    }
}

