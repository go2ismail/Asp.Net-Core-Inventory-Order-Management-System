using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.VendorCategoryManager.Queries;

public record GetVendorCategoryListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetVendorCategoryListProfile : Profile
{
    public GetVendorCategoryListProfile()
    {
        CreateMap<VendorCategory, GetVendorCategoryListDto>();
    }
}

public class GetVendorCategoryListResult
{
    public List<GetVendorCategoryListDto>? Data { get; init; }
}

public class GetVendorCategoryListRequest : IRequest<GetVendorCategoryListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetVendorCategoryListHandler : IRequestHandler<GetVendorCategoryListRequest, GetVendorCategoryListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetVendorCategoryListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetVendorCategoryListResult> Handle(GetVendorCategoryListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .VendorCategory
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetVendorCategoryListDto>>(entities);

        return new GetVendorCategoryListResult
        {
            Data = dtos
        };
    }


}



