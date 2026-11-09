using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DeliveryOrderManager.Queries;

public record GetDeliveryOrderListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? DeliveryDate { get; init; }
    public DeliveryOrderStatus? Status { get; init; }
    public string? StatusName { get; init; }
    public string? Description { get; init; }
    public string? SalesOrderId { get; init; }
    public string? SalesOrderNumber { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetDeliveryOrderListProfile : Profile
{
    public GetDeliveryOrderListProfile()
    {
        CreateMap<DeliveryOrder, GetDeliveryOrderListDto>()
            .ForMember(
                dest => dest.SalesOrderNumber,
                opt => opt.MapFrom(src => src.SalesOrder != null ? src.SalesOrder.Number : string.Empty)
            )
            .ForMember(
                dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToFriendlyName() : string.Empty)
            );

    }
}

public class GetDeliveryOrderListResult
{
    public List<GetDeliveryOrderListDto>? Data { get; init; }
}

public class GetDeliveryOrderListRequest : IRequest<GetDeliveryOrderListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetDeliveryOrderListHandler : IRequestHandler<GetDeliveryOrderListRequest, GetDeliveryOrderListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetDeliveryOrderListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetDeliveryOrderListResult> Handle(GetDeliveryOrderListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .DeliveryOrder
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.SalesOrder)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetDeliveryOrderListDto>>(entities);

        return new GetDeliveryOrderListResult
        {
            Data = dtos
        };
    }


}



