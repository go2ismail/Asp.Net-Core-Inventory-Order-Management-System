using Application.Common.Repositories;
using Domain.Entities;

namespace Infrastructure.SeedManager.Demos;

public class ProductGroupSeeder
{
    private readonly ICommandRepository<ProductGroup> _productGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductGroupSeeder(
        ICommandRepository<ProductGroup> productGroupRepository,
        IUnitOfWork unitOfWork
    )
    {
        _productGroupRepository = productGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var productGroups = new List<ProductGroup>
        {
            new ProductGroup { Name = "Hardware" },
            new ProductGroup { Name = "Networking" },
            new ProductGroup { Name = "Storage" },
            new ProductGroup { Name = "Device" },
            new ProductGroup { Name = "Software" },
            new ProductGroup { Name = "Service" }
        };

        foreach (var productGroup in productGroups)
        {
            await _productGroupRepository.CreateAsync(productGroup);
        }

        await _unitOfWork.SaveAsync();
    }
}
