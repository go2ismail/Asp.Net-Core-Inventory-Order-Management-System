using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DeliveryOrderManager.Queries;


public class GetDeliveryOrderSingleProfile : Profile
{
    public GetDeliveryOrderSingleProfile()
    {
    }
}

public class GetDeliveryOrderSingleResult
{
    public DeliveryOrder? Data { get; init; }
    public List<InventoryTransaction>? TransactionList { get; init; }
}

public class GetDeliveryOrderSingleRequest : IRequest<GetDeliveryOrderSingleResult>
{
    public string? Id { get; init; }
}

public class GetDeliveryOrderSingleValidator : AbstractValidator<GetDeliveryOrderSingleRequest>
{
    public GetDeliveryOrderSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetDeliveryOrderSingleHandler : IRequestHandler<GetDeliveryOrderSingleRequest, GetDeliveryOrderSingleResult>
{
    private readonly IQueryContext _context;

    public GetDeliveryOrderSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetDeliveryOrderSingleResult> Handle(GetDeliveryOrderSingleRequest request, CancellationToken cancellationToken)
    {
        var queryData = _context
            .DeliveryOrder
            .AsNoTracking()
            .Include(x => x.SalesOrder)
                .ThenInclude(x => x.Customer)
            .Where(x => x.Id == request.Id)
            .AsQueryable();

        var data = await queryData.SingleOrDefaultAsync(cancellationToken);

        var queryTransactionList = _context
            .InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.Product)
            .Include(x => x.Warehouse)
            .Include(x => x.WarehouseFrom)
            .Include(x => x.WarehouseTo)
            .Where(x => x.ModuleId == request.Id && x.ModuleName == nameof(DeliveryOrder))
            .AsQueryable();

        var transactionList = await queryTransactionList.ToListAsync(cancellationToken);

        return new GetDeliveryOrderSingleResult
        {
            Data = data,
            TransactionList = transactionList
        };
    }
}