using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UnitMeasureManager.Queries;

public record GetUnitMeasureListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetUnitMeasureListProfile : Profile
{
    public GetUnitMeasureListProfile()
    {
        CreateMap<UnitMeasure, GetUnitMeasureListDto>();
    }
}

public class GetUnitMeasureListResult
{
    public List<GetUnitMeasureListDto>? Data { get; init; }
}

public class GetUnitMeasureListRequest : IRequest<GetUnitMeasureListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetUnitMeasureListHandler : IRequestHandler<GetUnitMeasureListRequest, GetUnitMeasureListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetUnitMeasureListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetUnitMeasureListResult> Handle(GetUnitMeasureListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .UnitMeasure
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetUnitMeasureListDto>>(entities);

        return new GetUnitMeasureListResult
        {
            Data = dtos
        };
    }


}



