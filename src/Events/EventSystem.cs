using System;
using System.Collections.Generic;

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

    public event Action<IReadOnlyList<GameEvent>>? TurnEventsResolved;

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
}
