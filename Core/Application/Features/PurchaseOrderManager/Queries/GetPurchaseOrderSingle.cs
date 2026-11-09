using Application.Common.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseOrderManager.Queries;


public class GetPurchaseOrderSingleProfile : Profile
{
    public GetPurchaseOrderSingleProfile()
    {
    }
}

public class GetPurchaseOrderSingleResult
{
    public PurchaseOrder? Data { get; init; }
}

public class GetPurchaseOrderSingleRequest : IRequest<GetPurchaseOrderSingleResult>
{
    public string? Id { get; init; }
}

public class GetPurchaseOrderSingleValidator : AbstractValidator<GetPurchaseOrderSingleRequest>
{
    public GetPurchaseOrderSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetPurchaseOrderSingleHandler : IRequestHandler<GetPurchaseOrderSingleRequest, GetPurchaseOrderSingleResult>
{
    private readonly IQueryContext _context;

    public GetPurchaseOrderSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetPurchaseOrderSingleResult> Handle(GetPurchaseOrderSingleRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .PurchaseOrder
            .AsNoTracking()
            .Include(x => x.Vendor)
            .Include(x => x.Tax)
            .Include(x => x.PurchaseOrderItemList.Where(item => !item.IsDeleted))
                .ThenInclude(x => x.Product)
            .Where(x => x.Id == request.Id)
            .AsQueryable();

        var entity = await query.SingleOrDefaultAsync(cancellationToken);

        return new GetPurchaseOrderSingleResult
        {
            Data = entity
        };
    }
}