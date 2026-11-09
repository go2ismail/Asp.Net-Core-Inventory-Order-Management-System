using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.SalesOrderManager.Commands;

public class DeleteSalesOrderResult
{
    public SalesOrder? Data { get; set; }
}

public class DeleteSalesOrderRequest : IRequest<DeleteSalesOrderResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteSalesOrderValidator : AbstractValidator<DeleteSalesOrderRequest>
{
    public DeleteSalesOrderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteSalesOrderHandler : IRequestHandler<DeleteSalesOrderRequest, DeleteSalesOrderResult>
{
    private readonly ICommandRepository<SalesOrder> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSalesOrderHandler(
        ICommandRepository<SalesOrder> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteSalesOrderResult> Handle(DeleteSalesOrderRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteSalesOrderResult
        {
            Data = entity
        };
    }
}

