using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Features.NumberSequenceManager;

public class NumberSequenceService
{

    private readonly object lockObject = new object();
    private readonly ICommandRepository<NumberSequence> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public NumberSequenceService(
        ICommandRepository<NumberSequence> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    private NumberSequence? GetNumberSequence(string entityName, string prefix, string suffix)
    {
        return _repository.GetQuery()
            .FirstOrDefault(ns => ns.EntityName == entityName && ns.Prefix == prefix && ns.Suffix == suffix);
    }

    private void UpdateNumberSequence(NumberSequence sequence)
    {
        sequence.LastUsedCount++;
        _unitOfWork.Save();
    }

    private NumberSequence InsertNumberSequence(string entityName, string prefix, string suffix)
    {
        NumberSequence newSequence = new NumberSequence
        {
            EntityName = entityName,
            Prefix = prefix,
            Suffix = suffix,
            LastUsedCount = 1
        };

        _repository.Create(newSequence);
        _unitOfWork.Save();

        return newSequence;
    }

    public string GenerateNumber(string entityName, string prefix, string suffix, bool useDate = true, int padding = 4)
    {
        var result = string.Empty;

        if (string.IsNullOrEmpty(entityName))
        {
            throw new Exception("Parameter entityName must not be null");
        }

        lock (lockObject)
        {
            NumberSequence? sequence = GetNumberSequence(entityName, prefix, suffix);

            if (sequence != null)
            {
                UpdateNumberSequence(sequence);
            }
            else
            {
                sequence = InsertNumberSequence(entityName, prefix, suffix);
            }

            string formattedNumber = $"{prefix}{sequence?.LastUsedCount?.ToString().PadLeft(padding, '0')}{(useDate ? DateTime.Now.ToString("yyyyMMdd") : "")}{suffix}";
            result = formattedNumber;
        }

        return result;
    }

}
