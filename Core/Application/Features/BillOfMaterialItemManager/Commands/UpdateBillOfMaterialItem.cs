using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.BillOfMaterialItemManager.Commands;

public class UpdateBillOfMaterialItemResult
{
    public BillOfMaterialItem? Data { get; set; }
}

public class UpdateBillOfMaterialItemRequest : IRequest<UpdateBillOfMaterialItemResult>
{
    public string? Id { get; init; }
    public double? Quantity { get; init; }
    public int? Sequence { get; init; }
    public string? UnitMeasureId { get; init; }
    public double? ScrapPercentage { get; init; }
    public string? Notes { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateBillOfMaterialItemValidator : AbstractValidator<UpdateBillOfMaterialItemRequest>
{
    public UpdateBillOfMaterialItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0).When(x => x.Quantity.HasValue);
        RuleFor(x => x.ScrapPercentage).InclusiveBetween(0, 100).When(x => x.ScrapPercentage.HasValue);
    }
}

public class UpdateBillOfMaterialItemHandler : IRequestHandler<UpdateBillOfMaterialItemRequest, UpdateBillOfMaterialItemResult>
{
    private readonly ICommandRepository<BillOfMaterialItem> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBillOfMaterialItemHandler(
        ICommandRepository<BillOfMaterialItem> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateBillOfMaterialItemResult> Handle(UpdateBillOfMaterialItemRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);
        if (entity == null)
        {
            throw new Exception($"BOM item not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;
        if (request.Quantity.HasValue) entity.Quantity = request.Quantity;
        if (request.Sequence.HasValue) entity.Sequence = request.Sequence;
        if (!string.IsNullOrWhiteSpace(request.UnitMeasureId)) entity.UnitMeasureId = request.UnitMeasureId;
        if (request.ScrapPercentage.HasValue) entity.ScrapPercentage = request.ScrapPercentage;
        if (request.Notes != null) entity.Notes = request.Notes;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateBillOfMaterialItemResult { Data = entity };
    }
}
