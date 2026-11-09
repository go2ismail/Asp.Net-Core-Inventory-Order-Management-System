using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CustomerCategoryManager.Queries;

public record GetCustomerCategoryListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetCustomerCategoryListProfile : Profile
{
    public GetCustomerCategoryListProfile()
    {
        CreateMap<CustomerCategory, GetCustomerCategoryListDto>();
    }
}

public class GetCustomerCategoryListResult
{
    public List<GetCustomerCategoryListDto>? Data { get; init; }
}

public class GetCustomerCategoryListRequest : IRequest<GetCustomerCategoryListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetCustomerCategoryListHandler : IRequestHandler<GetCustomerCategoryListRequest, GetCustomerCategoryListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetCustomerCategoryListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetCustomerCategoryListResult> Handle(GetCustomerCategoryListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .CustomerCategory
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetCustomerCategoryListDto>>(entities);

        return new GetCustomerCategoryListResult
        {
            Data = dtos
        };
    }


}



