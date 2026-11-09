using Application.Common.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SalesOrderManager.Queries;


public class GetSalesOrderSingleProfile : Profile
{
    public GetSalesOrderSingleProfile()
    {
    }
}

public class GetSalesOrderSingleResult
{
    public SalesOrder? Data { get; init; }
}

public class GetSalesOrderSingleRequest : IRequest<GetSalesOrderSingleResult>
{
    public string? Id { get; init; }
}

public class GetSalesOrderSingleValidator : AbstractValidator<GetSalesOrderSingleRequest>
{
    public GetSalesOrderSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetSalesOrderSingleHandler : IRequestHandler<GetSalesOrderSingleRequest, GetSalesOrderSingleResult>
{
    private readonly IQueryContext _context;

    public GetSalesOrderSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetSalesOrderSingleResult> Handle(GetSalesOrderSingleRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .SalesOrder
            .AsNoTracking()
            .Include(x => x.Customer)
            .Include(x => x.Tax)
            .Include(x => x.SalesOrderItemList.Where(item => !item.IsDeleted))
                .ThenInclude(x => x.Product)
            .Where(x => x.Id == request.Id)
            .AsQueryable();

        var entity = await query.SingleOrDefaultAsync(cancellationToken);

        return new GetSalesOrderSingleResult
        {
            Data = entity
        };
    }
}