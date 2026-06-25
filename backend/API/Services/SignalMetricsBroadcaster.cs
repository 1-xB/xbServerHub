using API.Hubs;
using Application.Interfaces;
using Domain.Entities.Metrics;
using Microsoft.AspNetCore.SignalR;

namespace API.Services;

public class SignalMetricsBroadcaster(IHubContext<MetricsHub> hubContext) : IMetricsBroadcaster
{
    public async Task BroadcastAsync(ServerMetrics serverMetrics, CancellationToken ct)
    {
        await hubContext.Clients.All.SendAsync("ReceiveMetrics", serverMetrics, ct);
    }
}