using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.BillOfMaterialManager.Commands;

public class UpdateBillOfMaterialResult
{
    public BillOfMaterial? Data { get; set; }
}

public class UpdateBillOfMaterialRequest : IRequest<UpdateBillOfMaterialResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Version { get; init; }
    public string? Description { get; init; }
    public bool? IsActive { get; init; }
    public DateTime? EffectiveFrom { get; init; }
    public DateTime? EffectiveTo { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateBillOfMaterialValidator : AbstractValidator<UpdateBillOfMaterialRequest>
{
    public UpdateBillOfMaterialValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).MaximumLength(255);
        RuleFor(x => x.Version).MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(4000);
        RuleFor(x => x.EffectiveTo)
            .GreaterThan(x => x.EffectiveFrom)
            .When(x => x.EffectiveFrom.HasValue && x.EffectiveTo.HasValue)
            .WithMessage("Effective To date must be greater than Effective From date");
    }
}

public class UpdateBillOfMaterialHandler : IRequestHandler<UpdateBillOfMaterialRequest, UpdateBillOfMaterialResult>
{
    private readonly ICommandRepository<BillOfMaterial> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBillOfMaterialHandler(
        ICommandRepository<BillOfMaterial> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateBillOfMaterialResult> Handle(UpdateBillOfMaterialRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;
        entity.Name = request.Name;
        entity.Version = request.Version;
        entity.Description = request.Description;
        entity.IsActive = request.IsActive;
        entity.EffectiveFrom = request.EffectiveFrom;
        entity.EffectiveTo = request.EffectiveTo;

        // Enforce only one active BOM per product when setting this to active
        if (request.IsActive == true && !string.IsNullOrEmpty(entity.ProductId))
        {
            var others = _repository.GetQuery()
                .Where(x => x.ProductId == entity.ProductId && x.Id != entity.Id && x.IsActive == true && !x.IsDeleted);
            foreach (var other in others)
            {
                other.IsActive = false;
                _repository.Update(other);
            }
        }

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateBillOfMaterialResult
        {
            Data = entity
        };
    }
}
