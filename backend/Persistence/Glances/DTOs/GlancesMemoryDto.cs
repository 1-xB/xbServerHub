using System.Text.Json.Serialization;

namespace Persistence.Glances.DTOs;

public class GlancesMemoryDto
{
    [JsonPropertyName("total")]
    public long Total { get; set; }

    [JsonPropertyName("available")]
    public long Available { get; set; }
    
    [JsonPropertyName("used")]
    public long Used { get; set; }
    
    [JsonPropertyName("percent")]
    public double Percent { get; set; }
}