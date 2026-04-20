using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.BillOfMaterialManager.Commands;

public class DeleteBillOfMaterialResult
{
    public BillOfMaterial? Data { get; set; }
}

public class DeleteBillOfMaterialRequest : IRequest<DeleteBillOfMaterialResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteBillOfMaterialValidator : AbstractValidator<DeleteBillOfMaterialRequest>
{
    public DeleteBillOfMaterialValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteBillOfMaterialHandler : IRequestHandler<DeleteBillOfMaterialRequest, DeleteBillOfMaterialResult>
{
    private readonly ICommandRepository<BillOfMaterial> _repository;
    private readonly ICommandRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBillOfMaterialHandler(
        ICommandRepository<BillOfMaterial> repository,
        ICommandRepository<Product> productRepository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteBillOfMaterialResult> Handle(DeleteBillOfMaterialRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);

        // If no other active/non-deleted BOMs remain for the product, unset HasBOM
        if (!string.IsNullOrEmpty(entity.ProductId))
        {
            var hasOther = _repository.GetQuery()
                .Any(x => x.ProductId == entity.ProductId && !x.IsDeleted && x.Id != entity.Id);
            if (!hasOther)
            {
                var product = await _productRepository.GetAsync(entity.ProductId, cancellationToken);
                if (product != null)
                {
                    product.HasBOM = false;
                    _productRepository.Update(product);
                }
            }
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteBillOfMaterialResult
        {
            Data = entity
        };
    }
}
