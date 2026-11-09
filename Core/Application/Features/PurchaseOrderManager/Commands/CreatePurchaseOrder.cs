using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.PurchaseOrderManager.Commands;

public class CreatePurchaseOrderResult
{
    public PurchaseOrder? Data { get; set; }
}

public class CreatePurchaseOrderRequest : IRequest<CreatePurchaseOrderResult>
{
    public DateTime? OrderDate { get; init; }
    public string? OrderStatus { get; init; }
    public string? Description { get; init; }
    public string? VendorId { get; init; }
    public string? TaxId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrderRequest>
{
    public CreatePurchaseOrderValidator()
    {
        RuleFor(x => x.OrderDate).NotEmpty();
        RuleFor(x => x.OrderStatus).NotEmpty();
        RuleFor(x => x.VendorId).NotEmpty();
        RuleFor(x => x.TaxId).NotEmpty();
    }
}

public class CreatePurchaseOrderHandler : IRequestHandler<CreatePurchaseOrderRequest, CreatePurchaseOrderResult>
{
    private readonly ICommandRepository<PurchaseOrder> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly PurchaseOrderService _purchaseOrderService;

    public CreatePurchaseOrderHandler(
        ICommandRepository<PurchaseOrder> repository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService,
        PurchaseOrderService purchaseOrderService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
        _purchaseOrderService = purchaseOrderService;
    }

    public async Task<CreatePurchaseOrderResult> Handle(CreatePurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new PurchaseOrder();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(PurchaseOrder), "", "PO");
        entity.OrderDate = request.OrderDate;
        entity.OrderStatus = (PurchaseOrderStatus)int.Parse(request.OrderStatus!);
        entity.Description = request.Description;
        entity.VendorId = request.VendorId;
        entity.TaxId = request.TaxId;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        _purchaseOrderService.Recalculate(entity.Id);

        return new CreatePurchaseOrderResult
        {
            Data = entity
        };
    }
}