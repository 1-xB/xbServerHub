namespace Domain.Entities.Metrics;

public class CpuMetrics
{
    public double UsagePercent { get; set; }
    public double UserPercent { get; set; }
    public double SystemPercent { get; set; }
}