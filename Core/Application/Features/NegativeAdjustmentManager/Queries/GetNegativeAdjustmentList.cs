using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.NegativeAdjustmentManager.Queries;

public record GetNegativeAdjustmentListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? AdjustmentDate { get; init; }
    public AdjustmentStatus? Status { get; init; }
    public string? StatusName { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetNegativeAdjustmentListProfile : Profile
{
    public GetNegativeAdjustmentListProfile()
    {
        CreateMap<NegativeAdjustment, GetNegativeAdjustmentListDto>()
            .ForMember(
                dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToFriendlyName() : string.Empty)
            );

    }
}

public class GetNegativeAdjustmentListResult
{
    public List<GetNegativeAdjustmentListDto>? Data { get; init; }
}

public class GetNegativeAdjustmentListRequest : IRequest<GetNegativeAdjustmentListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetNegativeAdjustmentListHandler : IRequestHandler<GetNegativeAdjustmentListRequest, GetNegativeAdjustmentListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetNegativeAdjustmentListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetNegativeAdjustmentListResult> Handle(GetNegativeAdjustmentListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .NegativeAdjustment
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetNegativeAdjustmentListDto>>(entities);

        return new GetNegativeAdjustmentListResult
        {
            Data = dtos
        };
    }


}



