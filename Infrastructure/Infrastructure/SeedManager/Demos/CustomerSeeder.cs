using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class CustomerSeeder
{
    private readonly ICommandRepository<Customer> _customerRepository;
    private readonly ICommandRepository<CustomerGroup> _groupRepository;
    private readonly ICommandRepository<CustomerCategory> _categoryRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerSeeder(
        ICommandRepository<Customer> customerRepository,
        ICommandRepository<CustomerGroup> groupRepository,
        ICommandRepository<CustomerCategory> categoryRepository,
        NumberSequenceService numberSequenceService,
        IUnitOfWork unitOfWork
    )
    {
        _customerRepository = customerRepository;
        _groupRepository = groupRepository;
        _categoryRepository = categoryRepository;
        _numberSequenceService = numberSequenceService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var groups = (await _groupRepository.GetQuery().ToListAsync()).Select(x => x.Id).ToArray();
        var categories = (await _categoryRepository.GetQuery().ToListAsync()).Select(x => x.Id).ToArray();
        var cities = new string[] { "New York", "Los Angeles", "San Francisco", "Chicago" };
        var streets = new string[] { "Main St", "Broadway", "Market St", "Elm St" };
        var states = new string[] { "NY", "CA", "IL", "TX" };
        var zipCodes = new string[] { "10001", "90001", "94101", "60601" };
        var phoneNumbers = new string[] { "555-1234", "555-5678", "555-8765", "555-4321" };
        var emailDomains = new string[] { "example.com", "demo.com", "test.com", "sample.com" };

        var random = new Random();

        var customers = new List<Customer>
        {
            new Customer { Name = "Citadel LLC" },
            new Customer { Name = "Ironclad LLC" },
            new Customer { Name = "Armada LLC" },
            new Customer { Name = "Shield LLC" },
            new Customer { Name = "Alpha LLC" },
            new Customer { Name = "Capitol LLC" },
            new Customer { Name = "Federal LLC" },
            new Customer { Name = "Statewide LLC" },
            new Customer { Name = "Harmony LLC" },
            new Customer { Name = "Hope LLC" },
            new Customer { Name = "Unity LLC" },
            new Customer { Name = "Prosperity LLC" },
            new Customer { Name = "Global LLC" },
            new Customer { Name = "Sunset LLC" },
            new Customer { Name = "Luxe LLC" },
            new Customer { Name = "Serenity LLC" },
            new Customer { Name = "Oasis LLC" },
            new Customer { Name = "Grandeur LLC" },
            new Customer { Name = "Bright LLC" },
            new Customer { Name = "Stellar LLC" }
        };

        foreach (var customer in customers)
        {
            customer.Number = _numberSequenceService.GenerateNumber(nameof(Customer), "", "CST");
            customer.CustomerGroupId = GetRandomValue(groups, random);
            customer.CustomerCategoryId = GetRandomValue(categories, random);
            customer.City = GetRandomString(cities, random);
            customer.Street = GetRandomString(streets, random);
            customer.State = GetRandomString(states, random);
            customer.ZipCode = GetRandomString(zipCodes, random);
            customer.PhoneNumber = GetRandomString(phoneNumbers, random);
            customer.EmailAddress = $"{customer.Name?.Split(' ')[0].ToLower()}@{GetRandomString(emailDomains, random)}";

            await _customerRepository.CreateAsync(customer);
        }

        await _unitOfWork.SaveAsync();
    }

    private static T GetRandomValue<T>(T[] array, Random random)
    {
        return array[random.Next(array.Length)];
    }

    private static string GetRandomString(string[] array, Random random)
    {
        return array[random.Next(array.Length)];
    }
}
