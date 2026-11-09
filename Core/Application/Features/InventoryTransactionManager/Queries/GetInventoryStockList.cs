using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.InventoryTransactionManager.Queries;


public record GetInventoryStockListDto
{
    public string? StatusName { get; init; }
    public string? WarehouseId { get; set; }
    public string? WarehouseName { get; init; }
    public string? ProductId { get; set; }
    public string? ProductName { get; init; }
    public string? ProductNumber { get; init; }
    public double? Stock { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}


public class GetInventoryStockListProfile : Profile
{
    public GetInventoryStockListProfile()
    {
    }
}

public class GetInventoryStockListResult
{
    public List<GetInventoryStockListDto>? Data { get; init; }
}

public class GetInventoryStockListRequest : IRequest<GetInventoryStockListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetInventoryStockListHandler : IRequestHandler<GetInventoryStockListRequest, GetInventoryStockListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetInventoryStockListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetInventoryStockListResult> Handle(GetInventoryStockListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Warehouse)
            .Include(x => x.Product)
            .Where(x =>
                x.Product!.Physical == true &&
                x.Warehouse!.SystemWarehouse == false &&
                x.Status == Domain.Enums.InventoryTransactionStatus.Confirmed
            )
            .GroupBy(x => new { x.WarehouseId, x.ProductId })
            .Select(group => new GetInventoryStockListDto
            {
                WarehouseId = group.Key.WarehouseId,
                ProductId = group.Key.ProductId,
                WarehouseName = group.Max(x => x.Warehouse!.Name),
                ProductName = group.Max(x => x.Product!.Name),
                ProductNumber = group.Max(x => x.Product!.Number),
                Stock = group.Sum(x => x.Stock),
                StatusName = group.Max(x => x.Status.ToString()),
                CreatedAtUtc = group.Max(x => x.CreatedAtUtc)
            })
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        return new GetInventoryStockListResult
        {
            Data = entities
        };
    }


}



