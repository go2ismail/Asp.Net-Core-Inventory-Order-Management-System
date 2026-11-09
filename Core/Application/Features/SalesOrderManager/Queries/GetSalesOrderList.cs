using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SalesOrderManager.Queries;

public record GetSalesOrderListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? OrderDate { get; init; }
    public SalesOrderStatus? OrderStatus { get; init; }
    public string? OrderStatusName { get; init; }
    public string? Description { get; init; }
    public string? CustomerId { get; init; }
    public string? CustomerName { get; init; }
    public string? TaxId { get; init; }
    public string? TaxName { get; init; }
    public double? BeforeTaxAmount { get; init; }
    public double? TaxAmount { get; init; }
    public double? AfterTaxAmount { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetSalesOrderListProfile : Profile
{
    public GetSalesOrderListProfile()
    {
        CreateMap<SalesOrder, GetSalesOrderListDto>()
            .ForMember(
                dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : string.Empty)
            )
            .ForMember(
                dest => dest.TaxName,
                opt => opt.MapFrom(src => src.Tax != null ? src.Tax.Name : string.Empty)
            )
            .ForMember(
                dest => dest.OrderStatusName,
                opt => opt.MapFrom(src => src.OrderStatus.HasValue ? src.OrderStatus.Value.ToFriendlyName() : string.Empty)
            );

    }
}

public class GetSalesOrderListResult
{
    public List<GetSalesOrderListDto>? Data { get; init; }
}

public class GetSalesOrderListRequest : IRequest<GetSalesOrderListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetSalesOrderListHandler : IRequestHandler<GetSalesOrderListRequest, GetSalesOrderListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetSalesOrderListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetSalesOrderListResult> Handle(GetSalesOrderListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .SalesOrder
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Customer)
            .Include(x => x.Tax)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetSalesOrderListDto>>(entities);

        return new GetSalesOrderListResult
        {
            Data = dtos
        };
    }


}



