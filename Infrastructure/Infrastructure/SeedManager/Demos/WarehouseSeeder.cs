using Application.Common.Repositories;
using Domain.Entities;

namespace Infrastructure.SeedManager.Demos
{
    public class WarehouseSeeder
    {
        private readonly ICommandRepository<Warehouse> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public WarehouseSeeder(
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
                new Warehouse { Name = "New York" },
                new Warehouse { Name = "San Francisco" },
                new Warehouse { Name = "Chicago" },
                new Warehouse { Name = "Los Angeles" }
            };

            foreach (var warehouse in warehouses)
            {
                await _repository.CreateAsync(warehouse);
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
