namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Any system that reacts to resolved events implements this interface.
/// Called by EventSystem after all events for the turn are generated.
/// </summary>
public interface IEventHandler
{
    string HandlerId { get; }

    bool CanHandle(GameEvent gameEvent);

    void Handle(GameEvent gameEvent);
}
