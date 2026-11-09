using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PositiveAdjustmentManager.Queries;


public class GetPositiveAdjustmentSingleProfile : Profile
{
    public GetPositiveAdjustmentSingleProfile()
    {
    }
}

public class GetPositiveAdjustmentSingleResult
{
    public PositiveAdjustment? Data { get; init; }
    public List<InventoryTransaction>? TransactionList { get; init; }
}

public class GetPositiveAdjustmentSingleRequest : IRequest<GetPositiveAdjustmentSingleResult>
{
    public string? Id { get; init; }
}

public class GetPositiveAdjustmentSingleValidator : AbstractValidator<GetPositiveAdjustmentSingleRequest>
{
    public GetPositiveAdjustmentSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetPositiveAdjustmentSingleHandler : IRequestHandler<GetPositiveAdjustmentSingleRequest, GetPositiveAdjustmentSingleResult>
{
    private readonly IQueryContext _context;

    public GetPositiveAdjustmentSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetPositiveAdjustmentSingleResult> Handle(GetPositiveAdjustmentSingleRequest request, CancellationToken cancellationToken)
    {
        var queryData = _context
            .PositiveAdjustment
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
            .Where(x => x.ModuleId == request.Id && x.ModuleName == nameof(PositiveAdjustment))
            .AsQueryable();

        var transactionList = await queryTransactionList.ToListAsync(cancellationToken);

        return new GetPositiveAdjustmentSingleResult
        {
            Data = data,
            TransactionList = transactionList
        };
    }
}