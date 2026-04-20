using Application.Common.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BillOfMaterialManager.Queries;

public record GetBillOfMaterialByProductDto
{
    public string? Id { get; init; }
    public string? ProductId { get; init; }
    public string? ProductName { get; init; }
    public string? Name { get; init; }
    public string? Version { get; init; }
    public string? Description { get; init; }
    public bool? IsActive { get; init; }
    public DateTime? EffectiveFrom { get; init; }
    public DateTime? EffectiveTo { get; init; }
    public List<BillOfMaterialItemDto>? Items { get; init; }
}

public record BillOfMaterialItemDto
{
    public string? Id { get; init; }
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

public class GetBillOfMaterialByProductProfile : Profile
{
    public GetBillOfMaterialByProductProfile()
    {
        CreateMap<BillOfMaterial, GetBillOfMaterialByProductDto>()
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty)
            );

        CreateMap<BillOfMaterialItem, BillOfMaterialItemDto>()
            .ForMember(
                dest => dest.ComponentProductName,
                opt => opt.MapFrom(src => src.ComponentProduct != null ? src.ComponentProduct.Name : string.Empty)
            )
            .ForMember(
                dest => dest.ComponentProductNumber,
                opt => opt.MapFrom(src => src.ComponentProduct != null ? src.ComponentProduct.Number : string.Empty)
            )
            .ForMember(
                dest => dest.UnitMeasureName,
                opt => opt.MapFrom(src => src.UnitMeasure != null ? src.UnitMeasure.Name : string.Empty)
            );
    }
}

public class GetBillOfMaterialByProductResult
{
    public GetBillOfMaterialByProductDto? Data { get; init; }
}

public class GetBillOfMaterialByProductRequest : IRequest<GetBillOfMaterialByProductResult>
{
    public string? ProductId { get; init; }
    public bool GetActiveOnly { get; init; } = true;
}

public class GetBillOfMaterialByProductHandler : IRequestHandler<GetBillOfMaterialByProductRequest, GetBillOfMaterialByProductResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetBillOfMaterialByProductHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetBillOfMaterialByProductResult> Handle(GetBillOfMaterialByProductRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .BillOfMaterials
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Where(x => x.ProductId == request.ProductId)
            .Include(x => x.Product)
            .Include(x => x.Items!.Where(i => !i.IsDeleted))
                .ThenInclude(i => i.ComponentProduct)
            .Include(x => x.Items!.Where(i => !i.IsDeleted))
                .ThenInclude(i => i.UnitMeasure)
            .AsQueryable();

        if (request.GetActiveOnly)
        {
            query = query.Where(x => x.IsActive == true);
        }

        var entity = await query
            .OrderByDescending(x => x.IsActive)
            .ThenByDescending(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return new GetBillOfMaterialByProductResult
            {
                Data = null
            };
        }

        var dto = _mapper.Map<GetBillOfMaterialByProductDto>(entity);

        return new GetBillOfMaterialByProductResult
        {
            Data = dto
        };
    }
}
