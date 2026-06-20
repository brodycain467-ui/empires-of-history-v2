using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EmpiresOfHistoryV2.Map.Models;

public class NationData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("tag")]
    public string Tag { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = "#7a7a7a";

    [JsonPropertyName("government_type")]
    public string GovernmentType { get; set; } = string.Empty;

    [JsonPropertyName("capital_province_id")]
    public string CapitalProvinceId { get; set; } = string.Empty;

    [JsonPropertyName("tier")]
    public string Tier { get; set; } = string.Empty;

    [JsonPropertyName("population")]
    public long Population { get; set; }

    [JsonPropertyName("treasury")]
    public double Treasury { get; set; }

    [JsonPropertyName("flag_path")]
    public string FlagPath { get; set; } = string.Empty;

    [JsonPropertyName("historical_accuracy_score")]
    public double HistoricalAccuracyScore { get; set; } // PLACEHOLDER

    [JsonPropertyName("top_officials")]
    public List<OfficialData> TopOfficials { get; set; } = new(); // PLACEHOLDER

    [JsonPropertyName("founding_year")]
    public int FoundingYear { get; set; }

    [JsonPropertyName("dissolution_year")]
    public int? DissolutionYear { get; set; }
}

public class NationsDatabase
{
    [JsonPropertyName("schema_version")]
    public string SchemaVersion { get; set; } = "1.0.0";

    [JsonPropertyName("nations")]
    public List<NationData> Nations { get; set; } = new();
}
