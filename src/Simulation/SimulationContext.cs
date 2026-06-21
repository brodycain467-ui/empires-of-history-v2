using System;
using System.Collections.Generic;
using EmpiresOfHistoryV2.Events;
using EmpiresOfHistoryV2.Map.Models;

namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Immutable snapshot of game state passed to every system each tick.
/// Systems READ from this — they NEVER write to it directly.
/// Systems produce output by: raising events, updating their own internal state,
/// or writing back through GameManager properties.
/// </summary>
public class SimulationContext
{
    public int TurnNumber { get; init; }
    public DateTime GameDate { get; init; }
    public string? ActiveNationId { get; init; }

    public IReadOnlyList<NationData> AllNations { get; init; } = [];
    public IReadOnlyList<string> ActiveNationProvinceIds { get; init; } = [];

    /// <summary>Read-only access to the event history for GIA / intelligence systems.</summary>
    public EventHistoryLog EventHistory { get; init; } = new();

    /// <summary>Inject an event from within a tick — routed through EventSystem, not fired directly.</summary>
    public Action<GameEvent>? InjectEvent { get; init; }
}
