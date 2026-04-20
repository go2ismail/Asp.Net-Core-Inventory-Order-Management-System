using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.BillOfMaterialManager.Commands;

public class CreateBillOfMaterialResult
{
    public BillOfMaterial? Data { get; set; }
}

public class CreateBillOfMaterialRequest : IRequest<CreateBillOfMaterialResult>
{
    public string? ProductId { get; init; }
    public string? Name { get; init; }
    public string? Version { get; init; }
    public string? Description { get; init; }
    public bool? IsActive { get; init; } = true;
    public DateTime? EffectiveFrom { get; init; }
    public DateTime? EffectiveTo { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateBillOfMaterialValidator : AbstractValidator<CreateBillOfMaterialRequest>
{
    public CreateBillOfMaterialValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product is required");
        RuleFor(x => x.Name).MaximumLength(255);
        RuleFor(x => x.Version).MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(4000);
        RuleFor(x => x.EffectiveTo)
            .GreaterThan(x => x.EffectiveFrom)
            .When(x => x.EffectiveFrom.HasValue && x.EffectiveTo.HasValue)
            .WithMessage("Effective To date must be greater than Effective From date");
    }
}

public class CreateBillOfMaterialHandler : IRequestHandler<CreateBillOfMaterialRequest, CreateBillOfMaterialResult>
{
    private readonly ICommandRepository<BillOfMaterial> _repository;
    private readonly ICommandRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBillOfMaterialHandler(
        ICommandRepository<BillOfMaterial> repository,
        ICommandRepository<Product> productRepository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateBillOfMaterialResult> Handle(CreateBillOfMaterialRequest request, CancellationToken cancellationToken = default)
    {
        // Verify product exists
        var product = await _productRepository.GetAsync(request.ProductId ?? string.Empty, cancellationToken);
        if (product == null)
        {
            throw new Exception($"Product not found: {request.ProductId}");
        }

        var entity = new BillOfMaterial();
        entity.CreatedById = request.CreatedById;
        entity.ProductId = request.ProductId;
        entity.Name = request.Name;
        entity.Version = request.Version ?? "1.0";
        entity.Description = request.Description;
        entity.IsActive = request.IsActive;
        entity.EffectiveFrom = request.EffectiveFrom ?? DateTime.UtcNow;
        entity.EffectiveTo = request.EffectiveTo;

        // Enforce only one active BOM per product
        if (entity.IsActive == true)
        {
            var existingActives = _repository.GetQuery()
                .Where(x => x.ProductId == entity.ProductId && x.IsActive == true && !x.IsDeleted);
            foreach (var active in existingActives)
            {
                if (active.Id != entity.Id)
                {
                    active.IsActive = false;
                    _repository.Update(active);
                }
            }
        }

        // Mark product as having BOM
        if (product.HasBOM != true)
        {
            product.HasBOM = true;
            _productRepository.Update(product);
        }

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateBillOfMaterialResult
        {
            Data = entity
        };
    }
}
