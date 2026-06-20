using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

namespace EmpiresOfHistoryV2.Map.Systems;

public class BorderHistorySystem
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly List<BorderHistoryEntry> _entries = new();

    public void Load()
    {
        var filePath = Path.Combine(ProjectSettings.GlobalizePath("res://"), "data", "history", "border_history.json");
        if (!File.Exists(filePath))
        {
            _entries.Clear();
            return;
        }

        var json = File.ReadAllText(filePath);
        var payload = JsonSerializer.Deserialize<BorderHistoryDatabase>(json, _jsonOptions) ?? new BorderHistoryDatabase();

        _entries.Clear();
        _entries.AddRange(payload.Borders);
    }

    public IReadOnlyList<BorderHistoryEntry> GetBordersAtYear(int year)
    {
        return _entries.Where(entry => entry.History.Any(h => h.Year <= year)).ToList();
    }
}

public class BorderHistoryDatabase
{
    [JsonPropertyName("schema_version")]
    public string SchemaVersion { get; set; } = "1.0.0";

    [JsonPropertyName("borders")]
    public List<BorderHistoryEntry> Borders { get; set; } = new();
}

public class BorderHistoryEntry
{
    [JsonPropertyName("province_a")]
    public string ProvinceA { get; set; } = string.Empty;

    [JsonPropertyName("province_b")]
    public string ProvinceB { get; set; } = string.Empty;

    [JsonPropertyName("history")]
    public List<BorderYearState> History { get; set; } = new();
}

public class BorderYearState
{
    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; } = "active";
}
