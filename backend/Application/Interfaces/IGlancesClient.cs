using Domain.Entities.Metrics;

namespace Application.Interfaces;

public interface IGlancesClient
{
    Task<ServerMetrics?> GetServerMetricsAsync(CancellationToken ct);
}