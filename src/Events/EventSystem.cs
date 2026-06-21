using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Top-level facade. GameManager owns one instance.
/// UI and other systems interact only through this class.
/// </summary>
public class EventSystem
{
    public EventQueue Queue { get; } = new();
    public EventHistoryLog History { get; } = new();
    public EventResolver Resolver { get; } = new();
    public EventChainTracker ChainTracker { get; } = new();

    public event Action<IReadOnlyList<GameEvent>>? TurnEventsResolved;
    public event Action<EventActionResult>? ActionResolved;

    public void RegisterSource(IEventSource source) => Resolver.RegisterSource(source);
    public void UnregisterSource(IEventSource source) => Resolver.UnregisterSource(source);
    public void RegisterHandler(IEventHandler handler) => Resolver.RegisterHandler(handler);
    public void UnregisterHandler(IEventHandler handler) => Resolver.UnregisterHandler(handler);

    public IReadOnlyList<GameEvent> ProcessTurn(EventContext context)
    {
        var resolved = Resolver.Resolve(context, Queue);
        History.RecordRange(resolved);
        TurnEventsResolved?.Invoke(resolved);
        return resolved;
    }

    public void InjectEvent(GameEvent gameEvent)
    {
        History.Record(gameEvent);
        TurnEventsResolved?.Invoke([gameEvent]);
    }

    /// <summary>
    /// Called when player selects an action on an event popup.
    /// Records the resolution and optionally injects a follow-up event.
    /// </summary>
    public EventActionResult ResolveAction(string eventId, string actionId)
    {
        // Find the event in history
        var gameEvent = History.GetById(eventId);
        if (gameEvent == null)
            return new EventActionResult { EventId = eventId, ActionId = actionId, Success = false };

        var action = gameEvent.Actions.FirstOrDefault(a => a.ActionId == actionId);
        if (action == null)
            return new EventActionResult { EventId = eventId, ActionId = actionId, Success = false };

        gameEvent.IsRead = true;
        gameEvent.ResolvedActionId = actionId;

        var result = new EventActionResult
        {
            EventId = eventId,
            ActionId = actionId,
            ActionType = action.ActionType,
            FollowUpEventId = action.FollowUpEventId,
            Success = true
        };

        // If action has a follow-up, it will be injected next turn via chain tracker
        ChainTracker.RecordRaised(eventId, gameEvent.TurnNumber, gameEvent.ExpiresAfterTurns);

        ActionResolved?.Invoke(result);
        return result;
    }
}
