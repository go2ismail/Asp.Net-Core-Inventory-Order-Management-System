using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos
{
    public class ProductSeeder
    {
        private readonly ICommandRepository<Product> _productRepository;
        private readonly ICommandRepository<ProductGroup> _productGroupRepository;
        private readonly ICommandRepository<UnitMeasure> _unitMeasureRepository;
        private readonly NumberSequenceService _numberSequenceService;
        private readonly IUnitOfWork _unitOfWork;

        public ProductSeeder(
            ICommandRepository<Product> productRepository,
            ICommandRepository<ProductGroup> productGroupRepository,
            ICommandRepository<UnitMeasure> unitMeasureRepository,
            NumberSequenceService numberSequenceService,
            IUnitOfWork unitOfWork
        )
        {
            _productRepository = productRepository;
            _productGroupRepository = productGroupRepository;
            _unitMeasureRepository = unitMeasureRepository;
            _numberSequenceService = numberSequenceService;
            _unitOfWork = unitOfWork;
        }

        public async Task GenerateDataAsync()
        {
            var productGroups = await _productGroupRepository.GetQuery().ToListAsync();
            var measures = (await _unitMeasureRepository.GetQuery().Where(x => x.Name == "unit").ToListAsync()).Select(x => x.Id).ToArray();

            var groupMapping = new Dictionary<string, string>();

            foreach (var pg in productGroups)
            {
                if (!string.IsNullOrEmpty(pg.Name) && pg.Id != null)
                {
                    groupMapping.Add(pg.Name, pg.Id);
                }
            }


            var products = new List<Product>
            {
                // Hardware
                new Product { Name = "Dell Servers", UnitPrice = 5000.0, ProductGroupId = groupMapping["Hardware"] },
                new Product { Name = "Dell Desktop Computers", UnitPrice = 2000.0, ProductGroupId = groupMapping["Hardware"] },
                new Product { Name = "Dell Laptops", UnitPrice = 3000.0, ProductGroupId = groupMapping["Hardware"] },

                // Networking
                new Product { Name = "Network Cables", UnitPrice = 100.0, ProductGroupId = groupMapping["Networking"] },
                new Product { Name = "Routers and Switches", UnitPrice = 1000.0, ProductGroupId = groupMapping["Networking"] },
                new Product { Name = "Antennas and Signal Boosters", UnitPrice = 2000.0, ProductGroupId = groupMapping["Networking"] },
                new Product { Name = "Wifii", UnitPrice = 1000.0, ProductGroupId = groupMapping["Networking"] },

                // Storage
                new Product { Name = "HDD 500", UnitPrice = 500.0, ProductGroupId = groupMapping["Storage"] },
                new Product { Name = "HDD 1T", UnitPrice = 800.0, ProductGroupId = groupMapping["Storage"] },
                new Product { Name = "SSD 500", UnitPrice = 1000.0, ProductGroupId = groupMapping["Storage"] },
                new Product { Name = "SSD 1T", UnitPrice = 1500.0, ProductGroupId = groupMapping["Storage"] },

                // Device
                new Product { Name = "Dell Keyboard", UnitPrice = 700.0, ProductGroupId = groupMapping["Device"] },
                new Product { Name = "Dell Mouse", UnitPrice = 500.0, ProductGroupId = groupMapping["Device"] },
                new Product { Name = "Dell Monitor 27inch", UnitPrice = 1000.0, ProductGroupId = groupMapping["Device"] },
                new Product { Name = "Dell Monitor 32inch", UnitPrice = 1500.0, ProductGroupId = groupMapping["Device"] },
                new Product { Name = "Dell Webcams", UnitPrice = 500.0, ProductGroupId = groupMapping["Device"] },

                // Software
                new Product { Name = "D365 License", UnitPrice = 800.0, Physical = false, ProductGroupId = groupMapping["Software"] },

                // Service
                new Product { Name = "IT Security", UnitPrice = 500.0, Physical = false, ProductGroupId = groupMapping["Service"] },
                new Product { Name = "Discount", UnitPrice = -1, Physical = false, ProductGroupId = groupMapping["Service"] }
            };

            foreach (var product in products)
            {
                product.Number = _numberSequenceService.GenerateNumber(nameof(Product), "", "ART");
                product.UnitMeasureId = measures[0];
                product.Physical ??= true;

                await _productRepository.CreateAsync(product);
            }

            await _unitOfWork.SaveAsync();
        }

        private static T GetRandomValue<T>(T[] array, Random random)
        {
            return array[random.Next(array.Length)];
        }
    }
}
