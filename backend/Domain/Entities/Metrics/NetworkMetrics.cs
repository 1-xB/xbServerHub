namespace Domain.Entities.Metrics;

public class NetworkMetrics
{
    public string InterfaceName { get; set; } = string.Empty;
    public double DownloadSpeed { get; set; }
    public double DownloadTotal { get; set; }
    public double UploadSpeed { get; set; }
    public double UploadTotal { get; set; }
}