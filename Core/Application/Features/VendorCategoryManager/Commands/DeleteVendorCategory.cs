using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.VendorCategoryManager.Commands;

public class DeleteVendorCategoryResult
{
    public VendorCategory? Data { get; set; }
}

public class DeleteVendorCategoryRequest : IRequest<DeleteVendorCategoryResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteVendorCategoryValidator : AbstractValidator<DeleteVendorCategoryRequest>
{
    public DeleteVendorCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteVendorCategoryHandler : IRequestHandler<DeleteVendorCategoryRequest, DeleteVendorCategoryResult>
{
    private readonly ICommandRepository<VendorCategory> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVendorCategoryHandler(
        ICommandRepository<VendorCategory> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteVendorCategoryResult> Handle(DeleteVendorCategoryRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteVendorCategoryResult
        {
            Data = entity
        };
    }
}

