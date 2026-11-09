using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SalesOrderItemManager.Queries;

public record GetSalesOrderItemBySalesOrderIdListDto
{
    public string? Id { get; init; }
    public string? SalesOrderId { get; init; }
    public string? SalesOrderNumber { get; init; }
    public string? ProductId { get; init; }
    public string? ProductName { get; init; }
    public string? ProductNumber { get; init; }
    public string? Summary { get; init; }
    public double? UnitPrice { get; init; }
    public double? Quantity { get; init; }
    public double? Total { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetSalesOrderItemBySalesOrderIdListProfile : Profile
{
    public GetSalesOrderItemBySalesOrderIdListProfile()
    {
        CreateMap<SalesOrderItem, GetSalesOrderItemBySalesOrderIdListDto>()
            .ForMember(
                dest => dest.SalesOrderNumber,
                opt => opt.MapFrom(src => src.SalesOrder != null ? src.SalesOrder.Number : string.Empty)
            )
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty)
            )
            .ForMember(
                dest => dest.ProductNumber,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Number : string.Empty)
            );

    }
}

public class GetSalesOrderItemBySalesOrderIdListResult
{
    public List<GetSalesOrderItemBySalesOrderIdListDto>? Data { get; init; }
}

public class GetSalesOrderItemBySalesOrderIdListRequest : IRequest<GetSalesOrderItemBySalesOrderIdListResult>
{
    public string? SalesOrderId { get; init; }
}


public class GetSalesOrderItemBySalesOrderIdListHandler : IRequestHandler<GetSalesOrderItemBySalesOrderIdListRequest, GetSalesOrderItemBySalesOrderIdListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetSalesOrderItemBySalesOrderIdListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetSalesOrderItemBySalesOrderIdListResult> Handle(GetSalesOrderItemBySalesOrderIdListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .SalesOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.SalesOrder)
            .Include(x => x.Product)
            .Where(x => x.SalesOrderId == request.SalesOrderId)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetSalesOrderItemBySalesOrderIdListDto>>(entities);

        return new GetSalesOrderItemBySalesOrderIdListResult
        {
            Data = dtos
        };
    }


}



