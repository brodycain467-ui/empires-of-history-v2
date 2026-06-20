using System;
using System.Collections.Generic;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// The core event record. All systems produce and consume GameEvent instances.
/// </summary>
public class GameEvent
{
    public string EventId { get; init; } = string.Empty;
    public string SourceSystemId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? IconKey { get; init; }
    public EventCategory Category { get; init; } = EventCategory.General;
    public int ImportanceScore { get; init; } = 10;
    public EventImportance Importance => EventImportanceExtensions.FromScore(ImportanceScore);
    public string? TargetNationId { get; init; }
    public string? TargetProvinceId { get; init; }
    public IReadOnlyDictionary<string, string> Effects { get; init; } = new Dictionary<string, string>();
    public DateTime GameDate { get; init; }
    public int TurnNumber { get; init; }
    public bool IsRead { get; set; }
    public DateTime RealTimestamp { get; init; } = DateTime.UtcNow;
}
