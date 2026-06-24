using System.Net.Http.Json;
using Application.Interfaces;
using Domain.Entities.Metrics;
using Microsoft.Extensions.Logging;
using Persistence.Glances.DTOs;

namespace Persistence.Glances;

public class GlancesClient(HttpClient httpClient, ILogger<GlancesClient> logger) : IGlancesClient
{
    public async Task<ServerMetrics?> GetServerMetricsAsync(CancellationToken ct)
    {
        try
        {
            var cpuTask = httpClient.GetFromJsonAsync<GlancesCpuDto>("api/4/cpu", ct);
            var memTask = httpClient.GetFromJsonAsync<GlancesMemoryDto>("api/4/mem", ct);
            var netTask = httpClient.GetFromJsonAsync<List<GlancesNetworkInterfaceDto>>("api/4/network", ct);
            
            await Task.WhenAll(cpuTask, memTask, netTask); // wykonanie równoległe wszystkich zadań
            
            var cpuDto = cpuTask.Result;
            var memDto = memTask.Result;
            var netDto = netTask.Result;
            
            if (cpuDto == null || netDto == null || memDto == null)
            {
                logger.LogWarning("Glances return not complete data");
                return null;
            }
            logger.LogInformation("Glances return data: CPU: {CpuDto}, Memory: {MemDto}, Network: {NetDto}", cpuDto.Total, memDto.Available, netDto[1].DownloadSpeed);
            return MapToServerMetrics(cpuDto, memDto, netDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while getting metrics from Glances");
            return null;
        }
    }

    private static ServerMetrics MapToServerMetrics(
            GlancesCpuDto cpuDto,
            GlancesMemoryDto memoryDto,
            List<GlancesNetworkInterfaceDto> netDto
        )
    {
        
        return new()
        {
            Cpu = new()
            {
                UsagePercent = cpuDto.Total,
                UserPercent = cpuDto.User,
                SystemPercent = cpuDto.System
            },
            Memory = new()
            {
                AvailableBytes = memoryDto.Available,
                TotalBytes = memoryDto.Total,
                UsedBytes = memoryDto.Used,
                UsedPercent = memoryDto.Percent
            },
            NetworkInterfaces = netDto.Select(n => new NetworkMetrics
            {
                InterfaceName = n.InterfaceName,
                DownloadSpeed = n.DownloadSpeed,
                DownloadTotal = n.DownloadTotal,
                UploadSpeed = n.UploadSpeed,
                UploadTotal = n.UploadTotal
            }).ToList(),
            FetchedAtUtc = DateTime.UtcNow
        };
    }
}