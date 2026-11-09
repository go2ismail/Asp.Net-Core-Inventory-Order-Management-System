using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Application.Features.SalesOrderManager;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class SalesOrderSeeder
{
    private readonly SalesOrderService _salesOrderService;
    private readonly ICommandRepository<SalesOrder> _salesOrderRepository;
    private readonly ICommandRepository<SalesOrderItem> _salesOrderItemRepository;
    private readonly ICommandRepository<Customer> _customerRepository;
    private readonly ICommandRepository<Tax> _taxRepository;
    private readonly ICommandRepository<Product> _productRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly IUnitOfWork _unitOfWork;

    public SalesOrderSeeder(
        SalesOrderService salesOrderService,
        ICommandRepository<SalesOrder> salesOrderRepository,
        ICommandRepository<SalesOrderItem> salesOrderItemRepository,
        ICommandRepository<Customer> customerRepository,
        ICommandRepository<Tax> taxRepository,
        ICommandRepository<Product> productRepository,
        NumberSequenceService numberSequenceService,
        IUnitOfWork unitOfWork
    )
    {
        _salesOrderService = salesOrderService;
        _salesOrderRepository = salesOrderRepository;
        _salesOrderItemRepository = salesOrderItemRepository;
        _customerRepository = customerRepository;
        _taxRepository = taxRepository;
        _productRepository = productRepository;
        _numberSequenceService = numberSequenceService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var random = new Random();
        var customers = await _customerRepository.GetQuery().Select(x => x.Id).ToListAsync();
        var taxes = await _taxRepository.GetQuery().Select(x => x.Id).ToListAsync();
        var products = await _productRepository.GetQuery().ToListAsync();

        var dateFinish = DateTime.Now;
        var dateStart = new DateTime(dateFinish.AddMonths(-12).Year, dateFinish.AddMonths(-12).Month, 1);

        for (DateTime date = dateStart; date < dateFinish; date = date.AddMonths(1))
        {
            DateTime[] transactionDates = GetRandomDays(date.Year, date.Month, 6);

            foreach (DateTime transDate in transactionDates)
            {
                var salesOrder = new SalesOrder
                {
                    Number = _numberSequenceService.GenerateNumber(nameof(SalesOrder), "", "SO"),
                    OrderDate = transDate,
                    OrderStatus = (SalesOrderStatus)random.Next(0, Enum.GetNames(typeof(SalesOrderStatus)).Length),
                    CustomerId = GetRandomValue(customers, random),
                    TaxId = GetRandomValue(taxes, random),
                };
                await _salesOrderRepository.CreateAsync(salesOrder);

                int numberOfProducts = random.Next(3, 6);
                for (int i = 0; i < numberOfProducts; i++)
                {
                    var qty = random.Next(2, 5);
                    var product = products[random.Next(products.Count)];
                    var salesOrderItem = new SalesOrderItem
                    {
                        SalesOrderId = salesOrder.Id,
                        ProductId = product.Id,
                        Summary = product.Number,
                        UnitPrice = product.UnitPrice,
                        Quantity = qty,
                        Total = product.UnitPrice * qty
                    };
                    await _salesOrderItemRepository.CreateAsync(salesOrderItem);
                }

                await _unitOfWork.SaveAsync();

                _salesOrderService.Recalculate(salesOrder.Id);
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
