namespace EmpiresOfHistoryV2.Events;

public class EventActionResult
{
    public string EventId { get; init; } = string.Empty;
    public string ActionId { get; init; } = string.Empty;
    public EventActionType ActionType { get; init; }
    public string? FollowUpEventId { get; init; }
    public bool Success { get; init; } = true;
}
