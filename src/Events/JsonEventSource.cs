using System;
using System.Collections.Generic;
using System.Linq;
using EmpiresOfHistoryV2.Events.Definitions;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// JSON-driven event source. Loads events from <see cref="EventDefinition"/> definitions
/// and fires them based on cycle position (turn % 20 == cycle_position).
/// Events with cycle_position == -1 are not cycle-driven and will not fire through
/// this source; they must be triggered externally via EventSystem.InjectEvent().
/// Chain prerequisites are gated through <see cref="EventChainTracker"/>.
/// </summary>
public class JsonEventSource : IEventSource
{
    public string SourceSystemId => "JsonEventSource";

    private readonly Dictionary<string, EventDefinition> _definitions;
    private readonly EventChainTracker _chainTracker;
    private const int CycleLength = 20;

    public JsonEventSource(Dictionary<string, EventDefinition> definitions, EventChainTracker chainTracker)
    {
        _definitions = definitions;
        _chainTracker = chainTracker;
    }

    public IReadOnlyList<GameEvent> GenerateEvents(EventContext context)
    {
        var results = new List<GameEvent>();
        var modulo = context.TurnNumber % CycleLength;

        foreach (var def in _definitions.Values)
        {
            // Skip expired/non-repeatable events
            if (_chainTracker.HasBeenRaised(def.EventId) && !def.Repeatable) continue;

            // Prerequisite events must have been raised
            if (def.PrerequisiteEventIds.Count > 0 &&
                !_chainTracker.PrerequisitesMet(def.PrerequisiteEventIds, context.TurnNumber)) continue;

            // Check cycle-based firing
            bool shouldFire = def.CyclePosition >= 0 && def.CyclePosition == modulo;

            // Global events fire for all nations; targeted events only for active nation
            if (!shouldFire) continue;
            if (!def.IsGlobal && def.TargetNationId != null && def.TargetNationId != context.ActiveNationId) continue;

            results.Add(BuildGameEvent(def, context));
        }

        return results;
    }

    private GameEvent BuildGameEvent(EventDefinition def, EventContext context)
    {
        return new GameEvent
        {
            EventId = def.EventId,
            SourceSystemId = SourceSystemId,
            Title = def.Title,
            Description = def.Description,
            Category = ParseCategory(def.Category),
            ImportanceScore = def.ImportanceScore,
            BaseChance = def.BaseChance,
            HistoricalWeight = def.HistoricalWeight,
            IsGlobal = def.IsGlobal,
            Repeatable = def.Repeatable,
            RequiresPlayerChoice = def.RequiresPlayerChoice,
            TargetNationId = def.TargetNationId ?? context.ActiveNationId,
            TargetProvinceId = def.TargetProvinceId,
            Effects = def.Effects,
            ParentEventId = def.ParentEventId,
            ChildEventIds = def.ChildEventIds,
            PrerequisiteEventIds = def.PrerequisiteEventIds,
            ExpiresAfterTurns = def.ExpiresAfterTurns,
            Actions = def.Actions.Select(a => new EventAction
            {
                ActionId = a.ActionId,
                Label = a.Label,
                ActionType = Enum.TryParse<EventActionType>(a.ActionType, out var at) ? at : EventActionType.Respond,
                ProbabilityModifiers = a.ProbabilityModifiers,
                FollowUpEventId = a.FollowUpEventId
            }).ToList(),
            GameDate = context.GameDate,
            TurnNumber = context.TurnNumber,
            RealTimestamp = DateTime.UtcNow
        };
    }

    private static EventCategory ParseCategory(string value) =>
        Enum.TryParse<EventCategory>(value, out var cat) ? cat : EventCategory.General;
}
