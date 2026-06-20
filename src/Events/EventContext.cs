using System;
using System.Collections.Generic;
using EmpiresOfHistoryV2.Map.Models;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Snapshot of game state passed to IEventSource.GenerateEvents().
/// Sources read from this — they do not call GameManager directly.
/// </summary>
public class EventContext
{
    public int TurnNumber { get; init; }
    public DateTime GameDate { get; init; }
    public string? ActiveNationId { get; init; }
    public IReadOnlyList<NationData> AllNations { get; init; } = [];
    public IReadOnlyList<string> ActiveNationProvinceIds { get; init; } = [];
    public int TotalEventCount { get; init; }
}
