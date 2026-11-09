using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.NumberSequenceManager.Queries;

public record GetNumberSequenceListDto
{
    public string? Id { get; init; }
    public string? EntityName { get; init; }
    public string? Prefix { get; init; }
    public string? Suffix { get; init; }
    public string? LastUsedCount { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetNumberSequenceListProfile : Profile
{
    public GetNumberSequenceListProfile()
    {
        CreateMap<NumberSequence, GetNumberSequenceListDto>();
    }
}

public class GetNumberSequenceListResult
{
    public List<GetNumberSequenceListDto>? Data { get; init; }
}

public class GetNumberSequenceListRequest : IRequest<GetNumberSequenceListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetNumberSequenceListHandler : IRequestHandler<GetNumberSequenceListRequest, GetNumberSequenceListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetNumberSequenceListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetNumberSequenceListResult> Handle(GetNumberSequenceListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .NumberSequence
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetNumberSequenceListDto>>(entities);

        return new GetNumberSequenceListResult
        {
            Data = dtos
        };
    }


}



