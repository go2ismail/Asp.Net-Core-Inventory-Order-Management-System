using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SalesReturnManager.Queries;

public record GetSalesReturnListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? ReturnDate { get; init; }
    public SalesReturnStatus? Status { get; init; }
    public string? StatusName { get; init; }
    public string? Description { get; init; }
    public string? DeliveryOrderId { get; init; }
    public string? DeliveryOrderNumber { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetSalesReturnListProfile : Profile
{
    public GetSalesReturnListProfile()
    {
        CreateMap<SalesReturn, GetSalesReturnListDto>()
            .ForMember(
                dest => dest.DeliveryOrderNumber,
                opt => opt.MapFrom(src => src.DeliveryOrder != null ? src.DeliveryOrder.Number : string.Empty)
            )
            .ForMember(
                dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToFriendlyName() : string.Empty)
            );

    }
}

public class GetSalesReturnListResult
{
    public List<GetSalesReturnListDto>? Data { get; init; }
}

public class GetSalesReturnListRequest : IRequest<GetSalesReturnListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetSalesReturnListHandler : IRequestHandler<GetSalesReturnListRequest, GetSalesReturnListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetSalesReturnListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetSalesReturnListResult> Handle(GetSalesReturnListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .SalesReturn
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.DeliveryOrder)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetSalesReturnListDto>>(entities);

        return new GetSalesReturnListResult
        {
            Data = dtos
        };
    }


}



