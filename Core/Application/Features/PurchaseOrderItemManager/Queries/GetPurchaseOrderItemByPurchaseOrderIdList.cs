using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseOrderItemManager.Queries;

public record GetPurchaseOrderItemByPurchaseOrderIdListDto
{
    public string? Id { get; init; }
    public string? PurchaseOrderId { get; init; }
    public string? PurchaseOrderNumber { get; init; }
    public string? ProductId { get; init; }
    public string? ProductName { get; init; }
    public string? ProductNumber { get; init; }
    public string? Summary { get; init; }
    public double? UnitPrice { get; init; }
    public double? Quantity { get; init; }
    public double? Total { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetPurchaseOrderItemByPurchaseOrderIdListProfile : Profile
{
    public GetPurchaseOrderItemByPurchaseOrderIdListProfile()
    {
        CreateMap<PurchaseOrderItem, GetPurchaseOrderItemByPurchaseOrderIdListDto>()
            .ForMember(
                dest => dest.PurchaseOrderNumber,
                opt => opt.MapFrom(src => src.PurchaseOrder != null ? src.PurchaseOrder.Number : string.Empty)
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

public class GetPurchaseOrderItemByPurchaseOrderIdListResult
{
    public List<GetPurchaseOrderItemByPurchaseOrderIdListDto>? Data { get; init; }
}

public class GetPurchaseOrderItemByPurchaseOrderIdListRequest : IRequest<GetPurchaseOrderItemByPurchaseOrderIdListResult>
{
    public string? PurchaseOrderId { get; init; }
}


public class GetPurchaseOrderItemByPurchaseOrderIdListHandler : IRequestHandler<GetPurchaseOrderItemByPurchaseOrderIdListRequest, GetPurchaseOrderItemByPurchaseOrderIdListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetPurchaseOrderItemByPurchaseOrderIdListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetPurchaseOrderItemByPurchaseOrderIdListResult> Handle(GetPurchaseOrderItemByPurchaseOrderIdListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .PurchaseOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.PurchaseOrder)
            .Include(x => x.Product)
            .Where(x => x.PurchaseOrderId == request.PurchaseOrderId)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetPurchaseOrderItemByPurchaseOrderIdListDto>>(entities);

        return new GetPurchaseOrderItemByPurchaseOrderIdListResult
        {
            Data = dtos
        };
    }


}



