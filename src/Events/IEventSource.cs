using System.Collections.Generic;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Any game system that produces events implements this interface.
/// Called by EventSystem each turn.
/// </summary>
public interface IEventSource
{
    string SourceSystemId { get; }

    IReadOnlyList<GameEvent> GenerateEvents(EventContext context);
}
