using Application.Common.Repositories;
using Application.Features.SalesOrderManager;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.SalesOrderItemManager.Commands;

public class CreateSalesOrderItemResult
{
    public SalesOrderItem? Data { get; set; }
}

public class CreateSalesOrderItemRequest : IRequest<CreateSalesOrderItemResult>
{
    public string? SalesOrderId { get; init; }
    public string? ProductId { get; init; }
    public string? Summary { get; init; }
    public double? UnitPrice { get; init; }
    public double? Quantity { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateSalesOrderItemValidator : AbstractValidator<CreateSalesOrderItemRequest>
{
    public CreateSalesOrderItemValidator()
    {
        RuleFor(x => x.SalesOrderId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.UnitPrice).NotEmpty();
        RuleFor(x => x.Quantity).NotEmpty();
    }
}

public class CreateSalesOrderItemHandler : IRequestHandler<CreateSalesOrderItemRequest, CreateSalesOrderItemResult>
{
    private readonly ICommandRepository<SalesOrderItem> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SalesOrderService _salesOrderService;

    public CreateSalesOrderItemHandler(
        ICommandRepository<SalesOrderItem> repository,
        IUnitOfWork unitOfWork,
        SalesOrderService salesOrderService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _salesOrderService = salesOrderService;
    }

    public async Task<CreateSalesOrderItemResult> Handle(CreateSalesOrderItemRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new SalesOrderItem();
        entity.CreatedById = request.CreatedById;

        entity.SalesOrderId = request.SalesOrderId;
        entity.ProductId = request.ProductId;
        entity.Summary = request.Summary;
        entity.UnitPrice = request.UnitPrice;
        entity.Quantity = request.Quantity;

        entity.Total = entity.Quantity * entity.UnitPrice;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        _salesOrderService.Recalculate(entity.SalesOrderId ?? "");

        return new CreateSalesOrderItemResult
        {
            Data = entity
        };
    }
}