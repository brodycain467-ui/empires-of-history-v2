using System.Collections.Generic;
using System.Linq;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Ordered, searchable log of all resolved events this session.
/// Used by Event Archive Screen and GIA Advisor.
/// </summary>
public class EventHistoryLog
{
    private readonly LinkedList<GameEvent> _history = [];
    private const int MaxHistory = 1000;

    public IReadOnlyList<GameEvent> All => _history.ToList();
    public int Count => _history.Count;

    public void Record(GameEvent gameEvent)
    {
        _history.AddFirst(gameEvent);
        if (_history.Count > MaxHistory)
        {
            _history.RemoveLast();
        }
    }

    public void RecordRange(IEnumerable<GameEvent> events)
    {
        foreach (var gameEvent in events)
        {
            Record(gameEvent);
        }
    }

    public IReadOnlyList<GameEvent> GetByCategory(EventCategory category) => _history.Where(gameEvent => gameEvent.Category == category).ToList();

    /// <summary>
    /// Returns nation-scoped events plus global events with no target nation.
    /// </summary>
    public IReadOnlyList<GameEvent> GetByNation(string nationId) => _history.Where(gameEvent => gameEvent.TargetNationId == nationId || gameEvent.TargetNationId == null).ToList();

    public IReadOnlyList<GameEvent> GetByTurn(int turn) => _history.Where(gameEvent => gameEvent.TurnNumber == turn).ToList();

    public IReadOnlyList<GameEvent> GetUnread() => _history.Where(gameEvent => !gameEvent.IsRead).ToList();

    public IReadOnlyList<GameEvent> GetByImportance(EventImportance importance) => _history.Where(gameEvent => gameEvent.Importance == importance).ToList();

    public void MarkAllRead()
    {
        foreach (var gameEvent in _history)
        {
            gameEvent.IsRead = true;
        }
    }
}
