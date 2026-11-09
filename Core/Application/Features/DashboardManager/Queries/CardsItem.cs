namespace Application.Features.DashboardManager.Queries;

public class CardsItem
{
    public double? SalesTotal { get; init; }
    public double? SalesReturnTotal { get; init; }
    public double? PurchaseTotal { get; init; }
    public double? PurchaseReturnTotal { get; init; }
    public double? DeliveryOrderTotal { get; init; }
    public double? GoodsReceiveTotal { get; init; }
    public double? TransferOutTotal { get; init; }
    public double? TransferInTotal { get; init; }
}
