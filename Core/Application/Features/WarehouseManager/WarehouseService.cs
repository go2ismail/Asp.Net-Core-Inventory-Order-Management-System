using Application.Common.CQS.Queries;
using Domain.Entities;

namespace Application.Features.WarehouseManager;

public class WarehouseService
{
    private readonly IQueryContext _queryContext;

    public WarehouseService(
        IQueryContext queryContext
        )
    {
        _queryContext = queryContext;
    }


    public Warehouse? GetCustomerWarehouse()
    {
        return _queryContext.Set<Warehouse>().Where(x => x.Name == "Customer").FirstOrDefault();
    }

    public Warehouse? GetVendorWarehouse()
    {
        return _queryContext.Set<Warehouse>().Where(x => x.Name == "Vendor").FirstOrDefault();
    }

    public Warehouse? GetTransferWarehouse()
    {
        return _queryContext.Set<Warehouse>().Where(x => x.Name == "Transfer").FirstOrDefault();
    }

    public Warehouse? GetAdjustmentWarehouse()
    {
        return _queryContext.Set<Warehouse>().Where(x => x.Name == "Adjustment").FirstOrDefault();
    }

    public Warehouse? GetStockCountWarehouse()
    {
        return _queryContext.Set<Warehouse>().Where(x => x.Name == "StockCount").FirstOrDefault();
    }

    public Warehouse? GetScrappingWarehouse()
    {
        return _queryContext.Set<Warehouse>().Where(x => x.Name == "Scrapping").FirstOrDefault();
    }

}
