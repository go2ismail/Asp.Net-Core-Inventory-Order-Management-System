using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class VendorContactSeeder
{
    private readonly ICommandRepository<VendorContact> _vendorContactRepository;
    private readonly ICommandRepository<Vendor> _vendorRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly IUnitOfWork _unitOfWork;

    public VendorContactSeeder(
        ICommandRepository<VendorContact> vendorContactRepository,
        ICommandRepository<Vendor> vendorRepository,
        NumberSequenceService numberSequenceService,
        IUnitOfWork unitOfWork
    )
    {
        _vendorContactRepository = vendorContactRepository;
        _vendorRepository = vendorRepository;
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
            "Operations Manager", "Financial Planner", "Software Developer", "Vendor Success Manager",
            "Marketing Coordinator", "Quality Assurance Tester", "HR Specialist", "Event Coordinator",
            "Account Executive", "Network Administrator", "Sales Manager", "Legal Assistant"
        };

        var vendorIds = await _vendorRepository.GetQuery().Select(x => x.Id).ToListAsync();
        var random = new Random();

        var vendorContacts = new List<VendorContact>();

        foreach (var vendorId in vendorIds)
        {
            for (int i = 0; i < 3; i++)
            {
                var firstName = GetRandomString(firstNames, random);
                var lastName = GetRandomString(lastNames, random);

                vendorContacts.Add(new VendorContact
                {
                    Name = $"{firstName} {lastName}",
                    Number = _numberSequenceService.GenerateNumber(nameof(VendorContact), "", "VC"),
                    VendorId = vendorId,
                    JobTitle = GetRandomString(jobTitles, random),
                    EmailAddress = $"{firstName.ToLower()}.{lastName.ToLower()}@gmail.com",
                    PhoneNumber = $"+1-{random.Next(100, 999)}-{random.Next(100, 999)}-{random.Next(1000, 9999)}"
                });
            }
        }

        foreach (var contact in vendorContacts)
        {
            await _vendorContactRepository.CreateAsync(contact);
        }

        await _unitOfWork.SaveAsync();
    }

    private static string GetRandomString(string[] array, Random random)
    {
        return array[random.Next(array.Length)];
    }
}
