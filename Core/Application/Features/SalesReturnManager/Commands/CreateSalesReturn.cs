using Application.Common.Extensions;
using Application.Common.Repositories;
using Application.Features.InventoryTransactionManager;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SalesReturnManager.Commands;

public class CreateSalesReturnResult
{
    public SalesReturn? Data { get; set; }
}

public class CreateSalesReturnRequest : IRequest<CreateSalesReturnResult>
{
    public DateTime? ReturnDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? DeliveryOrderId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateSalesReturnHandler : IRequestHandler<CreateSalesReturnRequest, CreateSalesReturnResult>
{
    private readonly ICommandRepository<SalesReturn> _deliveryOrderRepository;
    private readonly ICommandRepository<InventoryTransaction> _itemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;

    public CreateSalesReturnHandler(
        ICommandRepository<SalesReturn> deliveryOrderRepository,
        ICommandRepository<InventoryTransaction> itemRepository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService
        )
    {
        _deliveryOrderRepository = deliveryOrderRepository;
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;
    }

    public async Task<CreateSalesReturnResult> Handle(CreateSalesReturnRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new SalesReturn();
        entity.CreatedById = request.CreatedById;

        entity.Number = _numberSequenceService.GenerateNumber(nameof(SalesReturn), "", "SRN");
        entity.ReturnDate = request.ReturnDate;
        entity.Status = (SalesReturnStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.DeliveryOrderId = request.DeliveryOrderId;

        await _deliveryOrderRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var items = await _itemRepository
            .GetQuery()
            .ApplyIsDeletedFilter(false)
            .Where(x => x.ModuleId == entity.DeliveryOrderId && x.ModuleName == nameof(DeliveryOrder))
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            if (item?.Product?.Physical ?? false)
            {
                await _inventoryTransactionService.SalesReturnCreateInvenTrans(
                    entity.Id,
                    item.WarehouseId,
                    item.ProductId,
                    item.Movement,
                    entity.CreatedById,
                    cancellationToken
                    );

            }
        }

        return new CreateSalesReturnResult
        {
            Data = entity
        };
    }
}