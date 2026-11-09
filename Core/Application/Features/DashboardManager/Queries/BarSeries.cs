namespace Application.Features.DashboardManager.Queries;

public class BarSeries
{
    public string Type { get; set; } = string.Empty;
    public string XName { get; set; } = string.Empty;
    public int Width { get; set; }
    public string YName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double ColumnSpacing { get; set; }
    public string TooltipMappingName { get; set; } = string.Empty;
    public List<BarDataItem> DataSource { get; set; } = new List<BarDataItem>();
}
