using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ScrappingManager.Queries;

public record GetScrappingListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? ScrappingDate { get; init; }
    public ScrappingStatus? Status { get; init; }
    public string? StatusName { get; init; }
    public string? Description { get; init; }
    public string? WarehouseId { get; init; }
    public string? WarehouseName { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetScrappingListProfile : Profile
{
    public GetScrappingListProfile()
    {
        CreateMap<Scrapping, GetScrappingListDto>()
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

public class GetScrappingListResult
{
    public List<GetScrappingListDto>? Data { get; init; }
}

public class GetScrappingListRequest : IRequest<GetScrappingListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetScrappingListHandler : IRequestHandler<GetScrappingListRequest, GetScrappingListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetScrappingListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetScrappingListResult> Handle(GetScrappingListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .Scrapping
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Warehouse)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetScrappingListDto>>(entities);

        return new GetScrappingListResult
        {
            Data = dtos
        };
    }


}



