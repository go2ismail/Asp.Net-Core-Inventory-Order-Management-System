using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BillOfMaterialManager.Queries;

public record GetBillOfMaterialListDto
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
    public int? ItemCount { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetBillOfMaterialListProfile : Profile
{
    public GetBillOfMaterialListProfile()
    {
        CreateMap<BillOfMaterial, GetBillOfMaterialListDto>()
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty)
            )
            .ForMember(
                dest => dest.ItemCount,
                opt => opt.MapFrom(src => src.Items != null ? src.Items.Count : 0)
            );
    }
}

public class GetBillOfMaterialListResult
{
    public List<GetBillOfMaterialListDto>? Data { get; init; }
}

public class GetBillOfMaterialListRequest : IRequest<GetBillOfMaterialListResult>
{
    public bool IsDeleted { get; init; } = false;
    public string? ProductId { get; init; }
    public bool? IsActive { get; init; }
}

public class GetBillOfMaterialListHandler : IRequestHandler<GetBillOfMaterialListRequest, GetBillOfMaterialListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetBillOfMaterialListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetBillOfMaterialListResult> Handle(GetBillOfMaterialListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .BillOfMaterials
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Product)
            .Include(x => x.Items)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.ProductId))
        {
            query = query.Where(x => x.ProductId == request.ProductId);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var entities = await query.OrderByDescending(x => x.CreatedAtUtc).ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetBillOfMaterialListDto>>(entities);

        return new GetBillOfMaterialListResult
        {
            Data = dtos
        };
    }
}
