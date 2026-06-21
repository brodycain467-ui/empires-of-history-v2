using System;
using System.Collections.Generic;
using Godot;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Runs all registered IEventSources, collects events, dispatches to IEventHandlers.
/// Called once per turn by TurnSystem.
/// </summary>
public class EventResolver
{
    private readonly List<IEventSource> _sources = [];
    private readonly List<IEventHandler> _handlers = [];

    public void RegisterSource(IEventSource source) => _sources.Add(source);
    public void UnregisterSource(IEventSource source) => _sources.Remove(source);
    public void RegisterHandler(IEventHandler handler) => _handlers.Add(handler);
    public void UnregisterHandler(IEventHandler handler) => _handlers.Remove(handler);

    public IReadOnlyList<GameEvent> Resolve(EventContext context, EventQueue queue)
    {
        foreach (var source in _sources)
        {
            try
            {
                var events = source.GenerateEvents(context);
                if (events is { Count: > 0 })
                {
                    queue.EnqueueRange(events);
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EventResolver] Source '{source.SourceSystemId}' threw: {ex.Message}");
            }
        }

        var resolved = queue.FlushSorted();

        foreach (var gameEvent in resolved)
        {
            foreach (var handler in _handlers)
            {
                if (!handler.CanHandle(gameEvent))
                {
                    continue;
                }

                try
                {
                    handler.Handle(gameEvent);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[EventResolver] Handler '{handler.HandlerId}' threw on event '{gameEvent.EventId}': {ex.Message}");
                }
            }
        }

        return resolved;
    }
}
