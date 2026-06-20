using System.Collections.Generic;
using System.Linq;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Thread-safe FIFO queue. Holds pending events before resolution.
/// After resolution, events move to EventHistoryLog.
/// </summary>
public class EventQueue
{
    private readonly Queue<GameEvent> _pending = new();
    private readonly object _lock = new();

    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _pending.Count;
            }
        }
    }

    public void Enqueue(GameEvent gameEvent)
    {
        lock (_lock)
        {
            _pending.Enqueue(gameEvent);
        }
    }

    public void EnqueueRange(IEnumerable<GameEvent> events)
    {
        lock (_lock)
        {
            foreach (var gameEvent in events)
            {
                _pending.Enqueue(gameEvent);
            }
        }
    }

    public IReadOnlyList<GameEvent> FlushSorted()
    {
        lock (_lock)
        {
            var all = _pending.ToList();
            _pending.Clear();
            return all.OrderByDescending(gameEvent => gameEvent.ImportanceScore).ToList();
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _pending.Clear();
        }
    }
}
