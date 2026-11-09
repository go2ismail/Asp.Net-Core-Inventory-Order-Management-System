using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.WarehouseManager.Commands;

public class UpdateWarehouseResult
{
    public Warehouse? Data { get; set; }
}

public class UpdateWarehouseRequest : IRequest<UpdateWarehouseResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateWarehouseValidator : AbstractValidator<UpdateWarehouseRequest>
{
    public UpdateWarehouseValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateWarehouseHandler : IRequestHandler<UpdateWarehouseRequest, UpdateWarehouseResult>
{
    private readonly ICommandRepository<Warehouse> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWarehouseHandler(
        ICommandRepository<Warehouse> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateWarehouseResult> Handle(UpdateWarehouseRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        if (entity.SystemWarehouse == true)
        {
            throw new Exception($"Updating system warehouse is not allowed.");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateWarehouseResult
        {
            Data = entity
        };
    }
}

