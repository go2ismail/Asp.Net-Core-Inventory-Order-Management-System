using Application.Common.Repositories;
using Application.Features.SalesOrderManager;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.SalesOrderItemManager.Commands;

public class UpdateSalesOrderItemResult
{
    public SalesOrderItem? Data { get; set; }
}

public class UpdateSalesOrderItemRequest : IRequest<UpdateSalesOrderItemResult>
{
    public string? Id { get; init; }
    public string? SalesOrderId { get; init; }
    public string? ProductId { get; init; }
    public string? Summary { get; init; }
    public double? UnitPrice { get; init; }
    public double? Quantity { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateSalesOrderItemValidator : AbstractValidator<UpdateSalesOrderItemRequest>
{
    public UpdateSalesOrderItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SalesOrderId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.UnitPrice).NotEmpty();
        RuleFor(x => x.Quantity).NotEmpty();
    }
}

public class UpdateSalesOrderItemHandler : IRequestHandler<UpdateSalesOrderItemRequest, UpdateSalesOrderItemResult>
{
    private readonly ICommandRepository<SalesOrderItem> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SalesOrderService _salesOrderService;

    public UpdateSalesOrderItemHandler(
        ICommandRepository<SalesOrderItem> repository,
        IUnitOfWork unitOfWork,
        SalesOrderService salesOrderService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _salesOrderService = salesOrderService;
    }

    public async Task<UpdateSalesOrderItemResult> Handle(UpdateSalesOrderItemRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.SalesOrderId = request.SalesOrderId;
        entity.ProductId = request.ProductId;
        entity.Summary = request.Summary;
        entity.UnitPrice = request.UnitPrice;
        entity.Quantity = request.Quantity;

        entity.Total = entity.UnitPrice * entity.Quantity;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        _salesOrderService.Recalculate(entity.SalesOrderId ?? "");

        return new UpdateSalesOrderItemResult
        {
            Data = entity
        };
    }
}

