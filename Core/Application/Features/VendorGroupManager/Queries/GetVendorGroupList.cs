using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.VendorGroupManager.Queries;

public record GetVendorGroupListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetVendorGroupListProfile : Profile
{
    public GetVendorGroupListProfile()
    {
        CreateMap<VendorGroup, GetVendorGroupListDto>();
    }
}

public class GetVendorGroupListResult
{
    public List<GetVendorGroupListDto>? Data { get; init; }
}

public class GetVendorGroupListRequest : IRequest<GetVendorGroupListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetVendorGroupListHandler : IRequestHandler<GetVendorGroupListRequest, GetVendorGroupListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetVendorGroupListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetVendorGroupListResult> Handle(GetVendorGroupListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .VendorGroup
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetVendorGroupListDto>>(entities);

        return new GetVendorGroupListResult
        {
            Data = dtos
        };
    }


}



