namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Importance tier — drives popup priority and feed ordering.
/// Raw score 1–100 maps to these tiers.
/// </summary>
public enum EventImportance
{
    Minor = 1,
    Moderate = 2,
    Major = 3,
    Critical = 4
}

public static class EventImportanceExtensions
{
    public static EventImportance FromScore(int score) => score switch
    {
        <= 25 => EventImportance.Minor,
        <= 50 => EventImportance.Moderate,
        <= 75 => EventImportance.Major,
        _ => EventImportance.Critical
    };
}
