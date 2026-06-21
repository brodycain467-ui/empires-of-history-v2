using System;
using System.Text.Json.Serialization;

namespace EmpiresOfHistoryV2.Core;

/// <summary>
/// Represents a single save slot's persisted data.
/// Serialized to/from user://saves/save_N.json.
/// </summary>
public class SaveData
{
    [JsonPropertyName("selected_nation_id")]
    public string SelectedNationId { get; set; } = string.Empty;

    [JsonPropertyName("current_turn")]
    public int CurrentTurn { get; set; }

    [JsonPropertyName("current_date")]
    public DateTime CurrentDate { get; set; }

    [JsonPropertyName("saved_at")]
    public DateTime SavedAt { get; set; }

    [JsonPropertyName("slot_number")]
    public int SlotNumber { get; set; }

    [JsonPropertyName("save_version")]
    public int SaveVersion { get; set; } = 1;

    // Legacy UI/save compatibility fields (existing SaveSystem/MainMenu/SaveLoadDialog usage)
    public int Slot { get; set; }
    public string SaveName { get; set; } = string.Empty;
    public string DateTime { get; set; } = string.Empty;
    public string GameDate { get; set; } = string.Empty;
}
