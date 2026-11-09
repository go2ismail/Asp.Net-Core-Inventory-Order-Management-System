using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.GoodsReceiveManager.Queries;


public class GetGoodsReceiveSingleProfile : Profile
{
    public GetGoodsReceiveSingleProfile()
    {
    }
}

public class GetGoodsReceiveSingleResult
{
    public GoodsReceive? Data { get; init; }
    public List<InventoryTransaction>? TransactionList { get; init; }
}

public class GetGoodsReceiveSingleRequest : IRequest<GetGoodsReceiveSingleResult>
{
    public string? Id { get; init; }
}

public class GetGoodsReceiveSingleValidator : AbstractValidator<GetGoodsReceiveSingleRequest>
{
    public GetGoodsReceiveSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetGoodsReceiveSingleHandler : IRequestHandler<GetGoodsReceiveSingleRequest, GetGoodsReceiveSingleResult>
{
    private readonly IQueryContext _context;

    public GetGoodsReceiveSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetGoodsReceiveSingleResult> Handle(GetGoodsReceiveSingleRequest request, CancellationToken cancellationToken)
    {
        var queryData = _context
            .GoodsReceive
            .AsNoTracking()
            .Include(x => x.PurchaseOrder)
                .ThenInclude(x => x.Vendor)
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
            .Where(x => x.ModuleId == request.Id && x.ModuleName == nameof(GoodsReceive))
            .AsQueryable();

        var transactionList = await queryTransactionList.ToListAsync(cancellationToken);

        return new GetGoodsReceiveSingleResult
        {
            Data = data,
            TransactionList = transactionList
        };
    }
}