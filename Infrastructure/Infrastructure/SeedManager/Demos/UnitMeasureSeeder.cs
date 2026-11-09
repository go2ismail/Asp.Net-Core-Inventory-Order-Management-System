using Application.Common.Repositories;
using Domain.Entities;

namespace Infrastructure.SeedManager.Demos;

public class UnitMeasureSeeder
{
    private readonly ICommandRepository<UnitMeasure> _unitMeasureRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnitMeasureSeeder(
        ICommandRepository<UnitMeasure> unitMeasureRepository,
        IUnitOfWork unitOfWork
    )
    {
        _unitMeasureRepository = unitMeasureRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var unitMeasures = new List<UnitMeasure>
        {
            new UnitMeasure { Name = "m" },
            new UnitMeasure { Name = "kg" },
            new UnitMeasure { Name = "hour" },
            new UnitMeasure { Name = "unit" },
            new UnitMeasure { Name = "pcs" }
        };

        foreach (var unitMeasure in unitMeasures)
        {
            await _unitMeasureRepository.CreateAsync(unitMeasure);
        }

        await _unitOfWork.SaveAsync();
    }
}
