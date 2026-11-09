using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.GoodsReceiveManager.Queries;

public record GetGoodsReceiveListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? ReceiveDate { get; init; }
    public GoodsReceiveStatus? Status { get; init; }
    public string? StatusName { get; init; }
    public string? Description { get; init; }
    public string? PurchaseOrderId { get; init; }
    public string? PurchaseOrderNumber { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetGoodsReceiveListProfile : Profile
{
    public GetGoodsReceiveListProfile()
    {
        CreateMap<GoodsReceive, GetGoodsReceiveListDto>()
            .ForMember(
                dest => dest.PurchaseOrderNumber,
                opt => opt.MapFrom(src => src.PurchaseOrder != null ? src.PurchaseOrder.Number : string.Empty)
            )
            .ForMember(
                dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToFriendlyName() : string.Empty)
            );

    }
}

public class GetGoodsReceiveListResult
{
    public List<GetGoodsReceiveListDto>? Data { get; init; }
}

public class GetGoodsReceiveListRequest : IRequest<GetGoodsReceiveListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetGoodsReceiveListHandler : IRequestHandler<GetGoodsReceiveListRequest, GetGoodsReceiveListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetGoodsReceiveListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetGoodsReceiveListResult> Handle(GetGoodsReceiveListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .GoodsReceive
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.PurchaseOrder)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetGoodsReceiveListDto>>(entities);

        return new GetGoodsReceiveListResult
        {
            Data = dtos
        };
    }


}



