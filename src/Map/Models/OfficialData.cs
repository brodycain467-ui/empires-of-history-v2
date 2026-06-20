using System.Text.Json.Serialization;

namespace EmpiresOfHistoryV2.Map.Models;

public class OfficialData
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
