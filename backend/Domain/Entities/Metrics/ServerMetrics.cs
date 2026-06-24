namespace Domain.Entities.Metrics;

public class ServerMetrics
{
    public CpuMetrics Cpu { get; set; }
    public MemoryMetrics Memory { get; set; }
    public List<NetworkMetrics> NetworkInterfaces { get; set; } = new();
    public DateTime FetchedAtUtc { get; set; }
}