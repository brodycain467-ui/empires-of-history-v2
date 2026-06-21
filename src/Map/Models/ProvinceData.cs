using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EmpiresOfHistoryV2.Map.Models;

public class ProvinceData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("coordinates")]
    public ProvinceCoordinates Coordinates { get; set; } = new();

    [JsonPropertyName("shape")]
    public string Shape { get; set; } = "polygon";

    [JsonPropertyName("vertices")]
    public float[][] Vertices { get; set; } = [];

    [JsonPropertyName("capital")]
    public bool Capital { get; set; }

    [JsonPropertyName("neighbors")]
    public List<string> Neighbors { get; set; } = new();

    [JsonPropertyName("terrain")]
    public string Terrain { get; set; } = string.Empty;

    [JsonPropertyName("area_sq_km")]
    public float AreaSqKm { get; set; }

    [JsonPropertyName("initial_population")]
    public long InitialPopulation { get; set; }

    [JsonPropertyName("owner_nation_id")]
    public string OwnerNationId { get; set; } = string.Empty;

    [JsonIgnore]
    public string CurrentOwnerId { get; set; } = string.Empty;
}

public class ProvinceCoordinates
{
    [JsonPropertyName("x")]
    public float X { get; set; }

    [JsonPropertyName("y")]
    public float Y { get; set; }

    [JsonPropertyName("latitude")]
    public float Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public float Longitude { get; set; }
}

public class ProvincesDatabase
{
    [JsonPropertyName("schema_version")]
    public string SchemaVersion { get; set; } = "1.0.0";

    [JsonPropertyName("provinces")]
    public List<ProvinceData> Provinces { get; set; } = new();
}
