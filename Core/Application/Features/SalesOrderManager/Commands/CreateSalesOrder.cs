using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.SalesOrderManager.Commands;

public class CreateSalesOrderResult
{
    public SalesOrder? Data { get; set; }
}

public class CreateSalesOrderRequest : IRequest<CreateSalesOrderResult>
{
    public DateTime? OrderDate { get; init; }
    public string? OrderStatus { get; init; }
    public string? Description { get; init; }
    public string? CustomerId { get; init; }
    public string? TaxId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateSalesOrderValidator : AbstractValidator<CreateSalesOrderRequest>
{
    public CreateSalesOrderValidator()
    {
        RuleFor(x => x.OrderDate).NotEmpty();
        RuleFor(x => x.OrderStatus).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.TaxId).NotEmpty();
    }
}

public class CreateSalesOrderHandler : IRequestHandler<CreateSalesOrderRequest, CreateSalesOrderResult>
{
    private readonly ICommandRepository<SalesOrder> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly SalesOrderService _salesOrderService;

    public CreateSalesOrderHandler(
        ICommandRepository<SalesOrder> repository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService,
        SalesOrderService salesOrderService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
        _salesOrderService = salesOrderService;
    }

    public async Task<CreateSalesOrderResult> Handle(CreateSalesOrderRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new SalesOrder();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(SalesOrder), "", "SO");
        entity.OrderDate = request.OrderDate;
        entity.OrderStatus = (SalesOrderStatus)int.Parse(request.OrderStatus!);
        entity.Description = request.Description;
        entity.CustomerId = request.CustomerId;
        entity.TaxId = request.TaxId;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        _salesOrderService.Recalculate(entity.Id);

        return new CreateSalesOrderResult
        {
            Data = entity
        };
    }
}