using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.BillOfMaterialItemManager.Commands;

public class DeleteBillOfMaterialItemResult
{
    public BillOfMaterialItem? Data { get; set; }
}

public class DeleteBillOfMaterialItemRequest : IRequest<DeleteBillOfMaterialItemResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteBillOfMaterialItemValidator : AbstractValidator<DeleteBillOfMaterialItemRequest>
{
    public DeleteBillOfMaterialItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteBillOfMaterialItemHandler : IRequestHandler<DeleteBillOfMaterialItemRequest, DeleteBillOfMaterialItemResult>
{
    private readonly ICommandRepository<BillOfMaterialItem> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBillOfMaterialItemHandler(
        ICommandRepository<BillOfMaterialItem> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteBillOfMaterialItemResult> Handle(DeleteBillOfMaterialItemRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);
        if (entity == null)
        {
            throw new Exception($"BOM item not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;
        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteBillOfMaterialItemResult { Data = entity };
    }
}
