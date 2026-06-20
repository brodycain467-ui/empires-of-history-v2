using System.Collections.Generic;
using System.Linq;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Ordered, searchable log of all resolved events this session.
/// Used by Event Archive Screen and GIA Advisor.
/// </summary>
public class EventHistoryLog
{
    private readonly LinkedList<GameEvent> _history = new();
    private readonly Dictionary<string, GameEvent> _byId = new();              // O(1) by EventId
    private readonly Dictionary<EventCategory, List<GameEvent>> _byCategory = new(); // O(1) by Category
    private readonly Dictionary<string, List<GameEvent>> _byNation = new();    // O(1) by TargetNationId
    private readonly Dictionary<int, List<GameEvent>> _byTurn = new();         // O(1) by TurnNumber
    private const int MaxHistory = 1000;
    private const string GlobalNationKey = "__global__";

    public IReadOnlyList<GameEvent> All => _history.ToList();
    public int Count => _history.Count;

    public void Record(GameEvent gameEvent)
    {
        _history.AddFirst(gameEvent);
        _byId[gameEvent.EventId] = gameEvent;
        AddToIndex(_byCategory, gameEvent.Category, gameEvent);
        AddToIndex(_byNation, gameEvent.TargetNationId ?? GlobalNationKey, gameEvent);
        AddToIndex(_byTurn, gameEvent.TurnNumber, gameEvent);

        if (_history.Count > MaxHistory)
        {
            var oldest = _history.Last!.Value;
            _history.RemoveLast();
            RemoveFromIndexes(oldest);
        }
    }

    public void RecordRange(IEnumerable<GameEvent> events)
    {
        foreach (var e in events) Record(e);
    }

    // O(1) lookups
    public GameEvent? GetById(string eventId) => _byId.GetValueOrDefault(eventId);
    public IReadOnlyList<GameEvent> GetByCategory(EventCategory category) =>
        _byCategory.TryGetValue(category, out var list) ? list : [];
    public IReadOnlyList<GameEvent> GetByTurn(int turn) =>
        _byTurn.TryGetValue(turn, out var list) ? list : [];
    public IReadOnlyList<GameEvent> GetByNation(string nationId)
    {
        var result = new List<GameEvent>();
        if (_byNation.TryGetValue(nationId, out var nation)) result.AddRange(nation);
        if (_byNation.TryGetValue(GlobalNationKey, out var global)) result.AddRange(global);
        return result;
    }
    public IReadOnlyList<GameEvent> GetByProvince(string provinceId) =>
        _history.Where(e => e.TargetProvinceId == provinceId).ToList(); // rare query, scan acceptable
    public IReadOnlyList<GameEvent> GetUnread() =>
        _history.Where(e => !e.IsRead).ToList();
    public IReadOnlyList<GameEvent> GetByImportance(EventImportance importance) =>
        _history.Where(e => e.Importance == importance).ToList();

    public void MarkAllRead()
    {
        foreach (var e in _history) e.IsRead = true;
    }

    private void RemoveFromIndexes(GameEvent e)
    {
        _byId.Remove(e.EventId);
        RemoveFromIndex(_byCategory, e.Category, e);
        RemoveFromIndex(_byNation, e.TargetNationId ?? GlobalNationKey, e);
        RemoveFromIndex(_byTurn, e.TurnNumber, e);
    }

    private static void AddToIndex<TKey>(Dictionary<TKey, List<GameEvent>> index, TKey key, GameEvent e)
        where TKey : notnull
    {
        if (!index.TryGetValue(key, out var list))
        {
            list = new List<GameEvent>();
            index[key] = list;
        }
        list.Insert(0, e); // newest first
    }

    private static void RemoveFromIndex<TKey>(Dictionary<TKey, List<GameEvent>> index, TKey key, GameEvent e)
        where TKey : notnull
    {
        if (index.TryGetValue(key, out var list))
        {
            list.Remove(e);
            if (list.Count == 0)
                index.Remove(key);
        }
    }
}
