using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class CustomerContactSeeder
{
    private readonly ICommandRepository<CustomerContact> _customerContactRepository;
    private readonly ICommandRepository<Customer> _customerRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerContactSeeder(
        ICommandRepository<CustomerContact> customerContactRepository,
        ICommandRepository<Customer> customerRepository,
        NumberSequenceService numberSequenceService,
        IUnitOfWork unitOfWork
    )
    {
        _customerContactRepository = customerContactRepository;
        _customerRepository = customerRepository;
        _numberSequenceService = numberSequenceService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var firstNames = new string[]
        {
            "Adam", "Sarah", "Michael", "Emily", "David", "Jessica",
            "Kevin", "Samantha", "Jason", "Olivia", "Matthew", "Ashley",
            "Christopher", "Jennifer", "Nicholas", "Amanda", "Alexander",
            "Stephanie", "Jonathan", "Lauren"
        };

        var lastNames = new string[]
        {
            "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis",
            "Garcia", "Rodriguez", "Wilson", "Martinez", "Anderson", "Taylor",
            "Thomas", "Hernandez", "Moore", "Martin", "Jackson", "Thompson",
            "White", "Lopez"
        };

        var jobTitles = new string[]
        {
            "Chief Executive Officer", "Data Scientist", "Product Manager", "Business Development Executive",
            "IT Consultant", "Social Media Specialist", "Research Analyst", "Content Writer",
            "Operations Manager", "Financial Planner", "Software Developer", "Customer Success Manager",
            "Marketing Coordinator", "Quality Assurance Tester", "HR Specialist", "Event Coordinator",
            "Account Executive", "Network Administrator", "Sales Manager", "Legal Assistant"
        };

        var customerIds = await _customerRepository.GetQuery().Select(x => x.Id).ToListAsync();
        var random = new Random();

        var customerContacts = new List<CustomerContact>();

        foreach (var customerId in customerIds)
        {
            for (int i = 0; i < 3; i++)
            {
                var firstName = GetRandomString(firstNames, random);
                var lastName = GetRandomString(lastNames, random);

                customerContacts.Add(new CustomerContact
                {
                    Name = $"{firstName} {lastName}",
                    Number = _numberSequenceService.GenerateNumber(nameof(CustomerContact), "", "CC"),
                    CustomerId = customerId,
                    JobTitle = GetRandomString(jobTitles, random),
                    EmailAddress = $"{firstName.ToLower()}.{lastName.ToLower()}@gmail.com",
                    PhoneNumber = GenerateRandomPhoneNumber(random)
                });
            }
        }

        foreach (var contact in customerContacts)
        {
            await _customerContactRepository.CreateAsync(contact);
        }

        await _unitOfWork.SaveAsync();
    }

    private static string GetRandomString(string[] array, Random random)
    {
        return array[random.Next(array.Length)];
    }

    private static string GenerateRandomPhoneNumber(Random random)
    {
        return $"+1-{random.Next(100, 999)}-{random.Next(100, 999)}-{random.Next(1000, 9999)}";
    }
}
