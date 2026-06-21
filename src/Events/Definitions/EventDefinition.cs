using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EmpiresOfHistoryV2.Events.Definitions;

public class EventDefinition
{
    [JsonPropertyName("event_id")]
    public string EventId { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = "General";

    [JsonPropertyName("importance_score")]
    public int ImportanceScore { get; set; } = 10;

    [JsonPropertyName("base_chance")]
    public double BaseChance { get; set; } = 1.0;

    [JsonPropertyName("historical_weight")]
    public double HistoricalWeight { get; set; } = 0.5;

    [JsonPropertyName("is_global")]
    public bool IsGlobal { get; set; } = false;

    [JsonPropertyName("repeatable")]
    public bool Repeatable { get; set; } = true;

    [JsonPropertyName("requires_player_choice")]
    public bool RequiresPlayerChoice { get; set; } = false;

    [JsonPropertyName("target_nation_id")]
    public string? TargetNationId { get; set; }

    [JsonPropertyName("target_province_id")]
    public string? TargetProvinceId { get; set; }

    [JsonPropertyName("effects")]
    public Dictionary<string, string> Effects { get; set; } = new();

    [JsonPropertyName("prerequisites")]
    public List<string> Prerequisites { get; set; } = new();

    [JsonPropertyName("follow_up_events")]
    public List<string> FollowUpEvents { get; set; } = new();

    // Event chain fields
    [JsonPropertyName("parent_event_id")]
    public string? ParentEventId { get; set; }

    [JsonPropertyName("child_event_ids")]
    public List<string> ChildEventIds { get; set; } = new();

    [JsonPropertyName("prerequisite_event_ids")]
    public List<string> PrerequisiteEventIds { get; set; } = new();

    [JsonPropertyName("expires_after_turns")]
    public int? ExpiresAfterTurns { get; set; }

    [JsonPropertyName("cycle_position")]
    public int CyclePosition { get; set; } = -1; // -1 = not cycle-driven

    [JsonPropertyName("actions")]
    public List<EventActionDefinition> Actions { get; set; } = new();
}

public class EventActionsDatabase
{
    [JsonPropertyName("schema_version")]
    public string SchemaVersion { get; set; } = "1.0.0";

    [JsonPropertyName("events")]
    public List<EventDefinition> Events { get; set; } = new();
}
