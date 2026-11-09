using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.StockCountManager.Queries;


public class GetStockCountSingleProfile : Profile
{
    public GetStockCountSingleProfile()
    {
    }
}

public class GetStockCountSingleResult
{
    public StockCount? Data { get; init; }
    public List<InventoryTransaction>? TransactionList { get; init; }
}

public class GetStockCountSingleRequest : IRequest<GetStockCountSingleResult>
{
    public string? Id { get; init; }
}

public class GetStockCountSingleValidator : AbstractValidator<GetStockCountSingleRequest>
{
    public GetStockCountSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetStockCountSingleHandler : IRequestHandler<GetStockCountSingleRequest, GetStockCountSingleResult>
{
    private readonly IQueryContext _context;

    public GetStockCountSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetStockCountSingleResult> Handle(GetStockCountSingleRequest request, CancellationToken cancellationToken)
    {
        var queryData = _context
            .StockCount
            .AsNoTracking()
            .Include(x => x.Warehouse)
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
            .Where(x => x.ModuleId == request.Id && x.ModuleName == nameof(StockCount))
            .AsQueryable();

        var transactionList = await queryTransactionList.ToListAsync(cancellationToken);

        return new GetStockCountSingleResult
        {
            Data = data,
            TransactionList = transactionList
        };
    }
}