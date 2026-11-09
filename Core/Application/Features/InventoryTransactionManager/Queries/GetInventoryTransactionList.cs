using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.InventoryTransactionManager.Queries;


public record GetInventoryTransactionListDto
{
    public string? Id { get; init; }
    public string? ModuleId { get; init; }
    public string? ModuleName { get; init; }
    public string? ModuleCode { get; init; }
    public string? ModuleNumber { get; init; }
    public DateTime? MovementDate { get; init; }
    public string? StatusName { get; init; }
    public string? Number { get; init; }
    public string? WarehouseName { get; init; }
    public string? ProductName { get; init; }
    public double? Movement { get; init; }
    public string? TransTypeName { get; init; }
    public double? Stock { get; init; }
    public string? WarehouseFromName { get; init; }
    public string? WarehouseToName { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}


public class GetInventoryTransactionListProfile : Profile
{
    public GetInventoryTransactionListProfile()
    {
        CreateMap<InventoryTransaction, GetInventoryTransactionListDto>()
            .ForMember(
                dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToFriendlyName() : string.Empty)
            )
            .ForMember(
                dest => dest.TransTypeName,
                opt => opt.MapFrom(src => src.TransType.HasValue ? src.TransType.Value.ToFriendlyName() : string.Empty)
            )
            .ForMember(
                dest => dest.WarehouseName,
                opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : string.Empty)
            )
            .ForMember(
                dest => dest.WarehouseToName,
                opt => opt.MapFrom(src => src.WarehouseTo != null ? src.WarehouseTo.Name : string.Empty)
            )
            .ForMember(
                dest => dest.WarehouseFromName,
                opt => opt.MapFrom(src => src.WarehouseFrom != null ? src.WarehouseFrom.Name : string.Empty)
            )
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Number + ' ' + src.Product.Name : string.Empty)
            );
    }
}

public class GetInventoryTransactionListResult
{
    public List<GetInventoryTransactionListDto>? Data { get; init; }
}

public class GetInventoryTransactionListRequest : IRequest<GetInventoryTransactionListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetInventoryTransactionListHandler : IRequestHandler<GetInventoryTransactionListRequest, GetInventoryTransactionListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetInventoryTransactionListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetInventoryTransactionListResult> Handle(GetInventoryTransactionListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Warehouse)
            .Include(x => x.Product)
            .Include(x => x.WarehouseFrom)
            .Include(x => x.WarehouseTo)
            .Where(x =>
                x.Product!.Physical == true &&
                x.Warehouse!.SystemWarehouse == false &&
                x.Status == Domain.Enums.InventoryTransactionStatus.Confirmed
            )
            .OrderByDescending(x => x.CreatedAtUtc)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetInventoryTransactionListDto>>(entities);

        return new GetInventoryTransactionListResult
        {
            Data = dtos
        };
    }


}



