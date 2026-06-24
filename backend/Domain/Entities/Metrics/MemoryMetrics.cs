namespace Domain.Entities.Metrics;

public class MemoryMetrics
{
    public long TotalBytes { get; set; }
    public long AvailableBytes { get; set; }
    public long UsedBytes { get; set; }
    public double UsedPercent { get; set; }
}