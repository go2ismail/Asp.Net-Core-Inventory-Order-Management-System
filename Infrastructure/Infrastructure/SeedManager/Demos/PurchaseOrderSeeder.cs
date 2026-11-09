using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Application.Features.PurchaseOrderManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class PurchaseOrderSeeder
{
    private readonly PurchaseOrderService _purchaseOrderService;
    private readonly ICommandRepository<PurchaseOrder> _purchaseOrderRepository;
    private readonly ICommandRepository<PurchaseOrderItem> _purchaseOrderItemRepository;
    private readonly ICommandRepository<Vendor> _vendorRepository;
    private readonly ICommandRepository<Tax> _taxRepository;
    private readonly ICommandRepository<Product> _productRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseOrderSeeder(
        PurchaseOrderService purchaseOrderService,
        ICommandRepository<PurchaseOrder> purchaseOrderRepository,
        ICommandRepository<PurchaseOrderItem> purchaseOrderItemRepository,
        ICommandRepository<Vendor> vendorRepository,
        ICommandRepository<Tax> taxRepository,
        ICommandRepository<Product> productRepository,
        NumberSequenceService numberSequenceService,
        IUnitOfWork unitOfWork
    )
    {
        _purchaseOrderService = purchaseOrderService;
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _vendorRepository = vendorRepository;
        _taxRepository = taxRepository;
        _productRepository = productRepository;
        _numberSequenceService = numberSequenceService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var random = new Random();
        var vendors = await _vendorRepository.GetQuery().Select(x => x.Id).ToListAsync();
        var taxes = await _taxRepository.GetQuery().Select(x => x.Id).ToListAsync();
        var products = await _productRepository.GetQuery().ToListAsync();

        var dateFinish = DateTime.Now;
        var dateStart = new DateTime(dateFinish.AddMonths(-12).Year, dateFinish.AddMonths(-12).Month, 1);

        for (DateTime date = dateStart; date < dateFinish; date = date.AddMonths(1))
        {
            DateTime[] transactionDates = GetRandomDays(date.Year, date.Month, 6);

            foreach (DateTime transDate in transactionDates)
            {
                var purchaseOrder = new PurchaseOrder
                {
                    Number = _numberSequenceService.GenerateNumber(nameof(PurchaseOrder), "", "PO"),
                    OrderDate = transDate,
                    OrderStatus = (PurchaseOrderStatus)random.Next(0, Enum.GetNames(typeof(PurchaseOrderStatus)).Length),
                    VendorId = GetRandomValue(vendors, random),
                    TaxId = GetRandomValue(taxes, random),
                };
                await _purchaseOrderRepository.CreateAsync(purchaseOrder);

                int numberOfProducts = random.Next(3, 6);
                for (int i = 0; i < numberOfProducts; i++)
                {
                    var product = products[random.Next(products.Count)];
                    var quantity = random.Next(20, 50);
                    var purchaseOrderItem = new PurchaseOrderItem
                    {
                        PurchaseOrderId = purchaseOrder.Id,
                        ProductId = product.Id,
                        Summary = product.Number,
                        UnitPrice = product.UnitPrice,
                        Quantity = quantity,
                        Total = product.UnitPrice * quantity
                    };
                    await _purchaseOrderItemRepository.CreateAsync(purchaseOrderItem);
                }

                await _unitOfWork.SaveAsync();

                _purchaseOrderService.Recalculate(purchaseOrder.Id);
            }
        }
    }

    private static T GetRandomValue<T>(List<T> list, Random random)
    {
        return list[random.Next(list.Count)];
    }

    private static DateTime[] GetRandomDays(int year, int month, int count)
    {
        var random = new Random();
        var daysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month)).ToList();
        var selectedDays = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int day = daysInMonth[random.Next(daysInMonth.Count)];
            selectedDays.Add(day);
            daysInMonth.Remove(day);
        }

        return selectedDays.Select(day => new DateTime(year, month, day)).ToArray();
    }
}
