using Application.Common.Repositories;
using Domain.Entities;

namespace Infrastructure.SeedManager.Systems
{
    public class SystemWarehouseSeeder
    {
        private readonly ICommandRepository<Warehouse> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SystemWarehouseSeeder(
            ICommandRepository<Warehouse> repository,
            IUnitOfWork unitOfWork
        )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task GenerateDataAsync()
        {
            var warehouses = new List<Warehouse>
            {
                new Warehouse { Name = "Customer", SystemWarehouse = true },
                new Warehouse { Name = "Vendor", SystemWarehouse = true },
                new Warehouse { Name = "Transfer", SystemWarehouse = true },
                new Warehouse { Name = "Adjustment", SystemWarehouse = true },
                new Warehouse { Name = "StockCount", SystemWarehouse = true },
                new Warehouse { Name = "Scrapping", SystemWarehouse = true }
            };

            foreach (var warehouse in warehouses)
            {
                await _repository.CreateAsync(warehouse);
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
