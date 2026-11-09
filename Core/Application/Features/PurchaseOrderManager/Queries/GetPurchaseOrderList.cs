using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseOrderManager.Queries;

public record GetPurchaseOrderListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? OrderDate { get; init; }
    public PurchaseOrderStatus? OrderStatus { get; init; }
    public string? OrderStatusName { get; init; }
    public string? Description { get; init; }
    public string? VendorId { get; init; }
    public string? VendorName { get; init; }
    public string? TaxId { get; init; }
    public string? TaxName { get; init; }
    public double? BeforeTaxAmount { get; init; }
    public double? TaxAmount { get; init; }
    public double? AfterTaxAmount { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetPurchaseOrderListProfile : Profile
{
    public GetPurchaseOrderListProfile()
    {
        CreateMap<PurchaseOrder, GetPurchaseOrderListDto>()
            .ForMember(
                dest => dest.VendorName,
                opt => opt.MapFrom(src => src.Vendor != null ? src.Vendor.Name : string.Empty)
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

public class GetPurchaseOrderListResult
{
    public List<GetPurchaseOrderListDto>? Data { get; init; }
}

public class GetPurchaseOrderListRequest : IRequest<GetPurchaseOrderListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetPurchaseOrderListHandler : IRequestHandler<GetPurchaseOrderListRequest, GetPurchaseOrderListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetPurchaseOrderListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetPurchaseOrderListResult> Handle(GetPurchaseOrderListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .PurchaseOrder
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Vendor)
            .Include(x => x.Tax)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetPurchaseOrderListDto>>(entities);

        return new GetPurchaseOrderListResult
        {
            Data = dtos
        };
    }


}



