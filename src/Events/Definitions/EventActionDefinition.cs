using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EmpiresOfHistoryV2.Events.Definitions;

public class EventActionDefinition
{
    [JsonPropertyName("action_id")]
    public string ActionId { get; set; } = string.Empty;

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("action_type")]
    public string ActionType { get; set; } = "Respond"; // Respond | Ignore | Delegate | ViewDetails

    [JsonPropertyName("probability_modifiers")]
    public Dictionary<string, double> ProbabilityModifiers { get; set; } = new();

    [JsonPropertyName("follow_up_event_id")]
    public string? FollowUpEventId { get; set; }
}
