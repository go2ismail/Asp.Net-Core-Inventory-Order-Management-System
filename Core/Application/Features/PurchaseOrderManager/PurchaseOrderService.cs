using Application.Common.Extensions;
using Application.Common.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseOrderManager;

public class PurchaseOrderService
{
    private readonly ICommandRepository<PurchaseOrder> _purchaseOrderRepository;
    private readonly ICommandRepository<PurchaseOrderItem> _purchaseOrderItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseOrderService(
        ICommandRepository<PurchaseOrder> purchaseOrderRepository,
        ICommandRepository<PurchaseOrderItem> purchaseOrderItemRepository,
        IUnitOfWork unitOfWork
        )
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _unitOfWork = unitOfWork;
    }

    public void Recalculate(string purchaseOrderId)
    {
        var purchaseOrder = _purchaseOrderRepository
            .GetQuery()
            .ApplyIsDeletedFilter()
            .Where(x => x.Id == purchaseOrderId)
            .Include(x => x.Tax)
            .SingleOrDefault();

        if (purchaseOrder == null)
            return;

        var purchaseOrderItems = _purchaseOrderItemRepository
            .GetQuery()
            .ApplyIsDeletedFilter()
            .Where(x => x.PurchaseOrderId == purchaseOrderId)
            .ToList();

        purchaseOrder.BeforeTaxAmount = purchaseOrderItems.Sum(x => x.Total ?? 0);

        var taxPercentage = purchaseOrder.Tax?.Percentage ?? 0;
        purchaseOrder.TaxAmount = (purchaseOrder.BeforeTaxAmount ?? 0) * taxPercentage / 100;

        purchaseOrder.AfterTaxAmount = (purchaseOrder.BeforeTaxAmount ?? 0) + (purchaseOrder.TaxAmount ?? 0);

        _purchaseOrderRepository.Update(purchaseOrder);
        _unitOfWork.Save();
    }
}
