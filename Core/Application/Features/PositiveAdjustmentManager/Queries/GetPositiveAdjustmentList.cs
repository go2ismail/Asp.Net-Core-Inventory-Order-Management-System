using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PositiveAdjustmentManager.Queries;

public record GetPositiveAdjustmentListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? AdjustmentDate { get; init; }
    public AdjustmentStatus? Status { get; init; }
    public string? StatusName { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetPositiveAdjustmentListProfile : Profile
{
    public GetPositiveAdjustmentListProfile()
    {
        CreateMap<PositiveAdjustment, GetPositiveAdjustmentListDto>()
            .ForMember(
                dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToFriendlyName() : string.Empty)
            );

    }
}

public class GetPositiveAdjustmentListResult
{
    public List<GetPositiveAdjustmentListDto>? Data { get; init; }
}

public class GetPositiveAdjustmentListRequest : IRequest<GetPositiveAdjustmentListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetPositiveAdjustmentListHandler : IRequestHandler<GetPositiveAdjustmentListRequest, GetPositiveAdjustmentListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetPositiveAdjustmentListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetPositiveAdjustmentListResult> Handle(GetPositiveAdjustmentListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .PositiveAdjustment
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetPositiveAdjustmentListDto>>(entities);

        return new GetPositiveAdjustmentListResult
        {
            Data = dtos
        };
    }


}



