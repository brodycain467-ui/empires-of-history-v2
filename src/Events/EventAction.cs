using System.Collections.Generic;

namespace EmpiresOfHistoryV2.Events;

public class EventAction
{
    public string ActionId { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public EventActionType ActionType { get; init; } = EventActionType.Respond;
    public IReadOnlyDictionary<string, double> ProbabilityModifiers { get; init; } = new Dictionary<string, double>();
    public string? FollowUpEventId { get; init; }
}

public enum EventActionType
{
    Respond,
    Ignore,
    Delegate,
    ViewDetails
}
