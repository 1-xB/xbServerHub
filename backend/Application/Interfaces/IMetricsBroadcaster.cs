using Domain.Entities.Metrics;

namespace Application.Interfaces;

public interface IMetricsBroadcaster
{
    Task BroadcastAsync(ServerMetrics serverMetrics, CancellationToken ct);
}