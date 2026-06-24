using System.Text.Json.Serialization;

namespace Persistence.Glances.DTOs;

public class GlancesCpuDto
{
    [JsonPropertyName("total")]
    public double Total { get; set; }
    
    [JsonPropertyName("user")]
    public double User { get; set; }
    
    [JsonPropertyName("system")]
    public double System { get; set; }
    
}