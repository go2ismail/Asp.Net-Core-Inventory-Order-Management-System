using Application.Common.Extensions;
using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.GoodsReceiveManager.Commands;

public class CreateGoodsReceiveResult
{
    public GoodsReceive? Data { get; set; }
}

public class CreateGoodsReceiveRequest : IRequest<CreateGoodsReceiveResult>
{
    public DateTime? ReceiveDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? PurchaseOrderId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateGoodsReceiveValidator : AbstractValidator<CreateGoodsReceiveRequest>
{
    public CreateGoodsReceiveValidator()
    {
        RuleFor(x => x.ReceiveDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
    }
}

public class CreateGoodsReceiveHandler : IRequestHandler<CreateGoodsReceiveRequest, CreateGoodsReceiveResult>
{
    private readonly ICommandRepository<GoodsReceive> _deliveryOrderRepository;
    private readonly ICommandRepository<PurchaseOrderItem> _purchaseOrderItemRepository;
    private readonly ICommandRepository<Warehouse> _warehouseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public CreateGoodsReceiveHandler(
        ICommandRepository<GoodsReceive> deliveryOrderRepository,
        ICommandRepository<PurchaseOrderItem> purchaseOrderItemRepository,
        ICommandRepository<Warehouse> warehouseRepository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _deliveryOrderRepository = deliveryOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<CreateGoodsReceiveResult> Handle(CreateGoodsReceiveRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new GoodsReceive();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(GoodsReceive), "", "GR");
        entity.ReceiveDate = request.ReceiveDate;
        entity.Status = (GoodsReceiveStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.PurchaseOrderId = request.PurchaseOrderId;

        await _deliveryOrderRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var defaultWarehouse = await _warehouseRepository
            .GetQuery()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.SystemWarehouse == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (defaultWarehouse != null)
        {
            var items = await _purchaseOrderItemRepository
                .GetQuery()
                .ApplyIsDeletedFilter(false)
                .Where(x => x.PurchaseOrderId == entity.PurchaseOrderId)
                .Include(x => x.Product)
                .ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                if (item?.Product?.Physical ?? false)
                {
                    await _inventoryTransactionService.GoodsReceiveCreateInvenTrans(
                        entity.Id,
                        defaultWarehouse.Id,
                        item.ProductId,
                        item.Quantity,
                        entity.CreatedById,
                        cancellationToken
                        );

                }
            }
        }

        return new CreateGoodsReceiveResult
        {
            Data = entity
        };
    }
}