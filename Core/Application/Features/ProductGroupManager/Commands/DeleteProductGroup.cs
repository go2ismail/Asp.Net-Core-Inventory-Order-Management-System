using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.ProductGroupManager.Commands;

public class DeleteProductGroupResult
{
    public ProductGroup? Data { get; set; }
}

public class DeleteProductGroupRequest : IRequest<DeleteProductGroupResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteProductGroupValidator : AbstractValidator<DeleteProductGroupRequest>
{
    public DeleteProductGroupValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteProductGroupHandler : IRequestHandler<DeleteProductGroupRequest, DeleteProductGroupResult>
{
    private readonly ICommandRepository<ProductGroup> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductGroupHandler(
        ICommandRepository<ProductGroup> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteProductGroupResult> Handle(DeleteProductGroupRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteProductGroupResult
        {
            Data = entity
        };
    }
}

