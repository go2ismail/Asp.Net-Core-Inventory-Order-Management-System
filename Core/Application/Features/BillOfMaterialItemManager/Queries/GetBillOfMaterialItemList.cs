using Application.Common.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BillOfMaterialItemManager.Queries;

public record GetBillOfMaterialItemListDto
{
    public string? Id { get; init; }
    public string? BillOfMaterialId { get; init; }
    public string? ComponentProductId { get; init; }
    public string? ComponentProductName { get; init; }
    public string? ComponentProductNumber { get; init; }
    public double? Quantity { get; init; }
    public int? Sequence { get; init; }
    public string? UnitMeasureId { get; init; }
    public string? UnitMeasureName { get; init; }
    public double? ScrapPercentage { get; init; }
    public string? Notes { get; init; }
}

public class GetBillOfMaterialItemListProfile : Profile
{
    public GetBillOfMaterialItemListProfile()
    {
        CreateMap<BillOfMaterialItem, GetBillOfMaterialItemListDto>()
            .ForMember(dest => dest.ComponentProductName, opt => opt.MapFrom(src => src.ComponentProduct != null ? src.ComponentProduct.Name : string.Empty))
            .ForMember(dest => dest.ComponentProductNumber, opt => opt.MapFrom(src => src.ComponentProduct != null ? src.ComponentProduct.Number : string.Empty))
            .ForMember(dest => dest.UnitMeasureName, opt => opt.MapFrom(src => src.UnitMeasure != null ? src.UnitMeasure.Name : string.Empty));
    }
}

public class GetBillOfMaterialItemListResult
{
    public List<GetBillOfMaterialItemListDto>? Data { get; init; }
}

public class GetBillOfMaterialItemListRequest : IRequest<GetBillOfMaterialItemListResult>
{
    public string? BillOfMaterialId { get; init; }
}

public class GetBillOfMaterialItemListHandler : IRequestHandler<GetBillOfMaterialItemListRequest, GetBillOfMaterialItemListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetBillOfMaterialItemListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetBillOfMaterialItemListResult> Handle(GetBillOfMaterialItemListRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.BillOfMaterialId))
        {
            return new GetBillOfMaterialItemListResult { Data = new List<GetBillOfMaterialItemListDto>() };
        }

        var entities = await _context.BillOfMaterialItems
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.BillOfMaterialId == request.BillOfMaterialId)
            .Include(x => x.ComponentProduct)
            .Include(x => x.UnitMeasure)
            .OrderBy(x => x.Sequence ?? int.MaxValue)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetBillOfMaterialItemListDto>>(entities);

        return new GetBillOfMaterialItemListResult { Data = dtos };
    }
}
