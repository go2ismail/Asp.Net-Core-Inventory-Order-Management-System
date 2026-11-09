using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseOrderItemManager.Queries;

public record GetPurchaseOrderItemListDto
{
    public string? Id { get; init; }
    public string? PurchaseOrderId { get; init; }
    public string? PurchaseOrderNumber { get; init; }
    public string? VendorName { get; init; }
    public string? ProductId { get; init; }
    public string? ProductName { get; init; }
    public string? ProductNumber { get; init; }
    public string? Summary { get; init; }
    public double? UnitPrice { get; init; }
    public double? Quantity { get; init; }
    public double? Total { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetPurchaseOrderItemListProfile : Profile
{
    public GetPurchaseOrderItemListProfile()
    {
        CreateMap<PurchaseOrderItem, GetPurchaseOrderItemListDto>()
            .ForMember(
                dest => dest.PurchaseOrderNumber,
                opt => opt.MapFrom(src => src.PurchaseOrder != null ? src.PurchaseOrder.Number : string.Empty)
            )
            .ForMember(
                dest => dest.VendorName,
                opt => opt.MapFrom(src => src.PurchaseOrder!.Vendor != null ? src.PurchaseOrder.Vendor.Name : string.Empty)
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

public class GetPurchaseOrderItemListResult
{
    public List<GetPurchaseOrderItemListDto>? Data { get; init; }
}

public class GetPurchaseOrderItemListRequest : IRequest<GetPurchaseOrderItemListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetPurchaseOrderItemListHandler : IRequestHandler<GetPurchaseOrderItemListRequest, GetPurchaseOrderItemListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetPurchaseOrderItemListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetPurchaseOrderItemListResult> Handle(GetPurchaseOrderItemListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .PurchaseOrderItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.PurchaseOrder)
                .ThenInclude(x => x!.Vendor)
            .Include(x => x.Product)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetPurchaseOrderItemListDto>>(entities);

        return new GetPurchaseOrderItemListResult
        {
            Data = dtos
        };
    }


}



