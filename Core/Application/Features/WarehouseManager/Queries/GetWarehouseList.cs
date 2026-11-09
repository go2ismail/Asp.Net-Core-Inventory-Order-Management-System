using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WarehouseManager.Queries;

public record GetWarehouseListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public bool? SystemWarehouse { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetWarehouseListProfile : Profile
{
    public GetWarehouseListProfile()
    {
        CreateMap<Warehouse, GetWarehouseListDto>();
    }
}

public class GetWarehouseListResult
{
    public List<GetWarehouseListDto>? Data { get; init; }
}

public class GetWarehouseListRequest : IRequest<GetWarehouseListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetWarehouseListHandler : IRequestHandler<GetWarehouseListRequest, GetWarehouseListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetWarehouseListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetWarehouseListResult> Handle(GetWarehouseListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .Warehouse
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetWarehouseListDto>>(entities);

        return new GetWarehouseListResult
        {
            Data = dtos
        };
    }


}



