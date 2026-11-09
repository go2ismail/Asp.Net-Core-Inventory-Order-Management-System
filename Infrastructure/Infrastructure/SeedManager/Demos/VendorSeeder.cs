using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class VendorSeeder
{
    private readonly ICommandRepository<Vendor> _vendorRepository;
    private readonly ICommandRepository<VendorGroup> _groupRepository;
    private readonly ICommandRepository<VendorCategory> _categoryRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly IUnitOfWork _unitOfWork;

    public VendorSeeder(
        ICommandRepository<Vendor> vendorRepository,
        ICommandRepository<VendorGroup> groupRepository,
        ICommandRepository<VendorCategory> categoryRepository,
        NumberSequenceService numberSequenceService,
        IUnitOfWork unitOfWork
    )
    {
        _vendorRepository = vendorRepository;
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
        var streets = new string[] { "Main Street", "Broadway", "Elm Street", "Maple Avenue" };
        var states = new string[] { "NY", "CA", "IL", "TX" };
        var zipCodes = new string[] { "10001", "90001", "60601", "73301" };
        var phoneNumbers = new string[] { "123-456-7890", "987-654-3210", "555-123-4567", "111-222-3333" };
        var emails = new string[] { "vendor1@example.com", "vendor2@example.com", "vendor3@example.com", "vendor4@example.com" };

        var random = new Random();

        var vendors = new List<Vendor>
        {
            new Vendor { Name = "Quantum Industries" },
            new Vendor { Name = "Apex Ventures" },
            new Vendor { Name = "Horizon Enterprises" },
            new Vendor { Name = "Nova Innovations" },
            new Vendor { Name = "Phoenix Holdings" },
            new Vendor { Name = "Titan Group" },
            new Vendor { Name = "Zenith Corporation" },
            new Vendor { Name = "Prime Solutions" },
            new Vendor { Name = "Cascade Enterprises" },
            new Vendor { Name = "Aurora Holdings" },
            new Vendor { Name = "Vanguard Industries" },
            new Vendor { Name = "Empyrean Ventures" },
            new Vendor { Name = "Genesis Corporation" },
            new Vendor { Name = "Equinox Enterprises" },
            new Vendor { Name = "Summit Holdings" },
            new Vendor { Name = "Sovereign Solutions" },
            new Vendor { Name = "Spectrum Corporation" },
            new Vendor { Name = "Elysium Enterprises" },
            new Vendor { Name = "Infinity Holdings" },
            new Vendor { Name = "Momentum Ventures" }
        };

        foreach (var vendor in vendors)
        {
            vendor.Number = _numberSequenceService.GenerateNumber(nameof(Vendor), "", "VND");
            vendor.VendorGroupId = GetRandomValue(groups, random);
            vendor.VendorCategoryId = GetRandomValue(categories, random);
            vendor.City = GetRandomString(cities, random);
            vendor.Street = GetRandomString(streets, random);
            vendor.State = GetRandomString(states, random);
            vendor.ZipCode = GetRandomString(zipCodes, random);
            vendor.PhoneNumber = GetRandomString(phoneNumbers, random);
            vendor.EmailAddress = GetRandomString(emails, random);

            await _vendorRepository.CreateAsync(vendor);
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
