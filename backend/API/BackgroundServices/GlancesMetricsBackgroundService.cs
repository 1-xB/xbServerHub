using Application.Handlers;

namespace API.BackgroundServices;

public class GlancesMetricsBackgroundService(IServiceScopeFactory scopeFactory, ILogger<GlancesMetricsBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("GlancesMetricsBackgroundService is starting.");
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(2));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<CollectSystemMetricsHandler>();
                await handler.HandleAsync(stoppingToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while collecting metrics from Glances");
            }
        }
    }
}