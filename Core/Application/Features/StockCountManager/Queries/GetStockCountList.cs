using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.StockCountManager.Queries;

public record GetStockCountListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? CountDate { get; init; }
    public StockCountStatus? Status { get; init; }
    public string? StatusName { get; init; }
    public string? Description { get; init; }
    public string? WarehouseId { get; init; }
    public string? WarehouseName { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetStockCountListProfile : Profile
{
    public GetStockCountListProfile()
    {
        CreateMap<StockCount, GetStockCountListDto>()
            .ForMember(
                dest => dest.WarehouseName,
                opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : string.Empty)
            )
            .ForMember(
                dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToFriendlyName() : string.Empty)
            );

    }
}

public class GetStockCountListResult
{
    public List<GetStockCountListDto>? Data { get; init; }
}

public class GetStockCountListRequest : IRequest<GetStockCountListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetStockCountListHandler : IRequestHandler<GetStockCountListRequest, GetStockCountListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetStockCountListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetStockCountListResult> Handle(GetStockCountListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .StockCount
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Warehouse)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetStockCountListDto>>(entities);

        return new GetStockCountListResult
        {
            Data = dtos
        };
    }


}



