using Application.Common.Repositories;
using Application.Features.BillOfMaterialManager;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.BillOfMaterialItemManager.Commands;

public class CreateBillOfMaterialItemResult
{
    public BillOfMaterialItem? Data { get; set; }
}

public class CreateBillOfMaterialItemRequest : IRequest<CreateBillOfMaterialItemResult>
{
    public string? BillOfMaterialId { get; init; }
    public string? ComponentProductId { get; init; }
    public double? Quantity { get; init; } = 1;
    public int? Sequence { get; init; }
    public string? UnitMeasureId { get; init; }
    public double? ScrapPercentage { get; init; } = 0;
    public string? Notes { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateBillOfMaterialItemValidator : AbstractValidator<CreateBillOfMaterialItemRequest>
{
    public CreateBillOfMaterialItemValidator()
    {
        RuleFor(x => x.BillOfMaterialId).NotEmpty();
        RuleFor(x => x.ComponentProductId).NotEmpty();
        RuleFor(x => x.Quantity).NotNull().GreaterThan(0);
        RuleFor(x => x.ScrapPercentage).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);
    }
}

public class CreateBillOfMaterialItemHandler : IRequestHandler<CreateBillOfMaterialItemRequest, CreateBillOfMaterialItemResult>
{
    private readonly ICommandRepository<BillOfMaterialItem> _repository;
    private readonly ICommandRepository<BillOfMaterial> _bomRepository;
    private readonly ICommandRepository<Product> _productRepository;
    private readonly BillOfMaterialService _bomService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBillOfMaterialItemHandler(
        ICommandRepository<BillOfMaterialItem> repository,
        ICommandRepository<BillOfMaterial> bomRepository,
        ICommandRepository<Product> productRepository,
        BillOfMaterialService bomService,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _bomRepository = bomRepository;
        _productRepository = productRepository;
        _bomService = bomService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateBillOfMaterialItemResult> Handle(CreateBillOfMaterialItemRequest request, CancellationToken cancellationToken)
    {
        // Validate BOM exists
        var bom = await _bomRepository.GetAsync(request.BillOfMaterialId ?? string.Empty, cancellationToken);
        if (bom == null)
        {
            throw new Exception($"BOM not found: {request.BillOfMaterialId}");
        }

        // Validate component product exists
        var componentProduct = await _productRepository.GetAsync(request.ComponentProductId ?? string.Empty, cancellationToken);
        if (componentProduct == null)
        {
            throw new Exception($"Component product not found: {request.ComponentProductId}");
        }

        // Circular reference validation
        var parentProductId = bom.ProductId ?? string.Empty;
        var componentProductId = request.ComponentProductId ?? string.Empty;
        var isCircular = await _bomService.ValidateCircularReferenceAsync(parentProductId, componentProductId, cancellationToken);
        if (isCircular)
        {
            throw new ValidationException($"Circular reference detected: product {parentProductId} cannot contain component {componentProductId}");
        }

        // Auto-increment sequence if not provided
        int? sequence = request.Sequence;
        if (!sequence.HasValue)
        {
            var existingItems = _repository.GetQuery().Where(x => x.BillOfMaterialId == request.BillOfMaterialId && !x.IsDeleted);
            var maxSeq = existingItems.Any() ? existingItems.Max(x => x.Sequence ?? 0) : 0;
            sequence = maxSeq + 1;
        }

        var entity = new BillOfMaterialItem
        {
            CreatedById = request.CreatedById,
            BillOfMaterialId = request.BillOfMaterialId,
            ComponentProductId = request.ComponentProductId,
            Quantity = request.Quantity ?? 1,
            Sequence = sequence,
            UnitMeasureId = request.UnitMeasureId,
            ScrapPercentage = request.ScrapPercentage ?? 0,
            Notes = request.Notes
        };

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateBillOfMaterialItemResult { Data = entity };
    }
}
