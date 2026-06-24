using System.Text.Json.Serialization;

namespace Persistence.Glances.DTOs;

public class GlancesNetworkInterfaceDto
{
    [JsonPropertyName("interface_name")]
    public string InterfaceName { get; set; }
    
    [JsonPropertyName("bytes_recv_rate_per_sec")]
    public double DownloadSpeed { get; set; }
    
    [JsonPropertyName("bytes_recv")]
    public double DownloadTotal { get; set; }
    
    [JsonPropertyName("bytes_sent_rate_per_sec")]
    public double UploadSpeed { get; set; }
    
    [JsonPropertyName("bytes_sent")]
    public double UploadTotal { get; set; }
}