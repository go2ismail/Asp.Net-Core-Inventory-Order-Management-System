using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SalesReturnManager.Queries;


public class GetSalesReturnSingleProfile : Profile
{
    public GetSalesReturnSingleProfile()
    {
    }
}

public class GetSalesReturnSingleResult
{
    public SalesReturn? Data { get; init; }
    public List<InventoryTransaction>? TransactionList { get; init; }
}

public class GetSalesReturnSingleRequest : IRequest<GetSalesReturnSingleResult>
{
    public string? Id { get; init; }
}

public class GetSalesReturnSingleValidator : AbstractValidator<GetSalesReturnSingleRequest>
{
    public GetSalesReturnSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetSalesReturnSingleHandler : IRequestHandler<GetSalesReturnSingleRequest, GetSalesReturnSingleResult>
{
    private readonly IQueryContext _context;

    public GetSalesReturnSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetSalesReturnSingleResult> Handle(GetSalesReturnSingleRequest request, CancellationToken cancellationToken)
    {
        var queryData = _context
            .SalesReturn
            .AsNoTracking()
            .Include(x => x.DeliveryOrder)
                .ThenInclude(x => x.SalesOrder)
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
            .Where(x => x.ModuleId == request.Id && x.ModuleName == nameof(SalesReturn))
            .AsQueryable();

        var transactionList = await queryTransactionList.ToListAsync(cancellationToken);

        return new GetSalesReturnSingleResult
        {
            Data = data,
            TransactionList = transactionList
        };
    }
}