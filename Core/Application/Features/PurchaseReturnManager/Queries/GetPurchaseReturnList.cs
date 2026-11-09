using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseReturnManager.Queries;

public record GetPurchaseReturnListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? ReturnDate { get; init; }
    public PurchaseReturnStatus? Status { get; init; }
    public string? StatusName { get; init; }
    public string? Description { get; init; }
    public string? GoodsReceiveId { get; init; }
    public string? GoodsReceiveNumber { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetPurchaseReturnListProfile : Profile
{
    public GetPurchaseReturnListProfile()
    {
        CreateMap<PurchaseReturn, GetPurchaseReturnListDto>()
            .ForMember(
                dest => dest.GoodsReceiveNumber,
                opt => opt.MapFrom(src => src.GoodsReceive != null ? src.GoodsReceive.Number : string.Empty)
            )
            .ForMember(
                dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToFriendlyName() : string.Empty)
            );

    }
}

public class GetPurchaseReturnListResult
{
    public List<GetPurchaseReturnListDto>? Data { get; init; }
}

public class GetPurchaseReturnListRequest : IRequest<GetPurchaseReturnListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetPurchaseReturnListHandler : IRequestHandler<GetPurchaseReturnListRequest, GetPurchaseReturnListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetPurchaseReturnListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetPurchaseReturnListResult> Handle(GetPurchaseReturnListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .PurchaseReturn
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.GoodsReceive)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetPurchaseReturnListDto>>(entities);

        return new GetPurchaseReturnListResult
        {
            Data = dtos
        };
    }


}



