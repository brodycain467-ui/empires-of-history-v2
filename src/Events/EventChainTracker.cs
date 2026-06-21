using System.Collections.Generic;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Tracks in-progress event chains across turns.
/// Future systems use this to gate follow-up events behind prerequisites.
/// </summary>
public class EventChainTracker
{
    // eventId → turn it was raised
    private readonly Dictionary<string, int> _raisedEvents = new();

    // eventId → turn it expires (null = never)
    private readonly Dictionary<string, int> _expiryByEvent = new();

    public void RecordRaised(string eventId, int turnNumber, int? expiresAfterTurns = null)
    {
        _raisedEvents[eventId] = turnNumber;
        if (expiresAfterTurns.HasValue)
            _expiryByEvent[eventId] = turnNumber + expiresAfterTurns.Value;
    }

    public bool HasBeenRaised(string eventId) => _raisedEvents.ContainsKey(eventId);

    public bool IsExpired(string eventId, int currentTurn)
    {
        if (!_expiryByEvent.TryGetValue(eventId, out var expiryTurn))
            return false;
        return currentTurn > expiryTurn;
    }

    /// <summary>
    /// Returns true if all prerequisite event IDs have been previously raised and not expired.
    /// </summary>
    public bool PrerequisitesMet(IEnumerable<string> prerequisiteEventIds, int currentTurn)
    {
        foreach (var prereq in prerequisiteEventIds)
        {
            if (!HasBeenRaised(prereq)) return false;
            if (IsExpired(prereq, currentTurn)) return false;
        }
        return true;
    }

    public void Clear() { _raisedEvents.Clear(); _expiryByEvent.Clear(); }
}
