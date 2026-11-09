using Application.Common.Repositories;
using Application.Features.SalesOrderManager;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.SalesOrderItemManager.Commands;

public class DeleteSalesOrderItemResult
{
    public SalesOrderItem? Data { get; set; }
}

public class DeleteSalesOrderItemRequest : IRequest<DeleteSalesOrderItemResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteSalesOrderItemValidator : AbstractValidator<DeleteSalesOrderItemRequest>
{
    public DeleteSalesOrderItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteSalesOrderItemHandler : IRequestHandler<DeleteSalesOrderItemRequest, DeleteSalesOrderItemResult>
{
    private readonly ICommandRepository<SalesOrderItem> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SalesOrderService _salesOrderService;

    public DeleteSalesOrderItemHandler(
        ICommandRepository<SalesOrderItem> repository,
        IUnitOfWork unitOfWork,
        SalesOrderService salesOrderService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _salesOrderService = salesOrderService;
    }

    public async Task<DeleteSalesOrderItemResult> Handle(DeleteSalesOrderItemRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        _salesOrderService.Recalculate(entity.SalesOrderId ?? "");

        return new DeleteSalesOrderItemResult
        {
            Data = entity
        };
    }
}

