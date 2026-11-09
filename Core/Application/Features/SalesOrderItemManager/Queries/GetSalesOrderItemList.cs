using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SalesOrderItemManager.Queries;

public record GetSalesOrderItemListDto
{
    public string? Id { get; init; }
    public string? SalesOrderId { get; init; }
    public string? SalesOrderNumber { get; init; }
    public string? CustomerName { get; init; }
    public string? ProductId { get; init; }
    public string? ProductName { get; init; }
    public string? ProductNumber { get; init; }
    public string? Summary { get; init; }
    public double? UnitPrice { get; init; }
    public double? Quantity { get; init; }
    public double? Total { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetSalesOrderItemListProfile : Profile
{
    public GetSalesOrderItemListProfile()
    {
        CreateMap<SalesOrderItem, GetSalesOrderItemListDto>()
            .ForMember(
                dest => dest.SalesOrderNumber,
                opt => opt.MapFrom(src => src.SalesOrder != null ? src.SalesOrder.Number : string.Empty)
            )
            .ForMember(
                dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.SalesOrder!.Customer != null ? src.SalesOrder.Customer.Name : string.Empty)
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

public class GetSalesOrderItemListResult
{
    public List<GetSalesOrderItemListDto>? Data { get; init; }
}

public class GetSalesOrderItemListRequest : IRequest<GetSalesOrderItemListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetSalesOrderItemListHandler : IRequestHandler<GetSalesOrderItemListRequest, GetSalesOrderItemListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetSalesOrderItemListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetSalesOrderItemListResult> Handle(GetSalesOrderItemListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .SalesOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.SalesOrder)
                .ThenInclude(x => x!.Customer)
            .Include(x => x.Product)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetSalesOrderItemListDto>>(entities);

        return new GetSalesOrderItemListResult
        {
            Data = dtos
        };
    }


}



