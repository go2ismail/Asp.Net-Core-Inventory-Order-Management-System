using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.NegativeAdjustmentManager.Queries;


public class GetNegativeAdjustmentSingleProfile : Profile
{
    public GetNegativeAdjustmentSingleProfile()
    {
    }
}

public class GetNegativeAdjustmentSingleResult
{
    public NegativeAdjustment? Data { get; init; }
    public List<InventoryTransaction>? TransactionList { get; init; }
}

public class GetNegativeAdjustmentSingleRequest : IRequest<GetNegativeAdjustmentSingleResult>
{
    public string? Id { get; init; }
}

public class GetNegativeAdjustmentSingleValidator : AbstractValidator<GetNegativeAdjustmentSingleRequest>
{
    public GetNegativeAdjustmentSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetNegativeAdjustmentSingleHandler : IRequestHandler<GetNegativeAdjustmentSingleRequest, GetNegativeAdjustmentSingleResult>
{
    private readonly IQueryContext _context;

    public GetNegativeAdjustmentSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetNegativeAdjustmentSingleResult> Handle(GetNegativeAdjustmentSingleRequest request, CancellationToken cancellationToken)
    {
        var queryData = _context
            .NegativeAdjustment
            .AsNoTracking()
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
            .Where(x => x.ModuleId == request.Id && x.ModuleName == nameof(NegativeAdjustment))
            .AsQueryable();

        var transactionList = await queryTransactionList.ToListAsync(cancellationToken);

        return new GetNegativeAdjustmentSingleResult
        {
            Data = data,
            TransactionList = transactionList
        };
    }
}