using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.WarehouseManager.Commands;

public class DeleteWarehouseResult
{
    public Warehouse? Data { get; set; }
}

public class DeleteWarehouseRequest : IRequest<DeleteWarehouseResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteWarehouseValidator : AbstractValidator<DeleteWarehouseRequest>
{
    public DeleteWarehouseValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteWarehouseHandler : IRequestHandler<DeleteWarehouseRequest, DeleteWarehouseResult>
{
    private readonly ICommandRepository<Warehouse> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWarehouseHandler(
        ICommandRepository<Warehouse> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteWarehouseResult> Handle(DeleteWarehouseRequest request, CancellationToken cancellationToken)
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

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteWarehouseResult
        {
            Data = entity
        };
    }
}

