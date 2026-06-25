using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers;

public class CollectSystemMetricsHandler(IGlancesClient glancesClient, ILogger<CollectSystemMetricsHandler> logger, IMetricsBroadcaster metricsBroadcaster) 
{
    public async Task HandleAsync(CancellationToken ct)
    {
        var serverMetrics = await glancesClient.GetServerMetricsAsync(ct);
        if (serverMetrics is null)
        {
            logger.LogWarning("Failed to collect system metrics.");
            return;
        }
        
        logger.LogInformation("Collected server metrics: {ServerMetrics}", serverMetrics);
        await metricsBroadcaster.BroadcastAsync(serverMetrics, ct);
    }
}