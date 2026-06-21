using System;
using System.Collections.Generic;

namespace EmpiresOfHistoryV2.Events;

/// <summary>
/// Placeholder event source — 20 template events covering future integration categories.
/// Replaced or supplemented by real system sources in future phases.
/// </summary>
public class PlaceholderEventSource : IEventSource
{
    private const int CycleLength = 20;

    private sealed record PlaceholderTemplate(
        string EventId,
        string Title,
        string Description,
        EventCategory Category,
        int ImportanceScore,
        int CyclePosition);

    private static readonly PlaceholderTemplate[] Templates =
    [
        new("evt_p01", "Domestic Reform Proposed", "A proposal for domestic reform has been submitted to the legislature.", EventCategory.Domestic, 30, 1),
        new("evt_p02", "Foreign Envoy Arrives", "An envoy from a neighboring nation has arrived seeking dialogue.", EventCategory.Foreign, 20, 2),
        new("evt_p03", "Economic Report Published", "The quarterly economic report has been released. Analysts project stable growth.", EventCategory.Economy, 25, 3),
        new("evt_p04", "Military Exercise Completed", "Scheduled military exercises have concluded without incident.", EventCategory.War, 15, 4),
        new("evt_p05", "Population Census Underway", "A national census has begun. Results expected next quarter.", EventCategory.Population, 20, 5),
        new("evt_p06", "Intelligence Report Filed", "The intelligence directorate has submitted its quarterly threat assessment.", EventCategory.Intelligence, 40, 6),
        new("evt_p07", "GIA Advisor Briefing", "Your GIA advisor has prepared a strategic overview for the current period.", EventCategory.GIA, 35, 7),
        new("evt_p08", "Historical Accuracy Review", "Historians have assessed this period against the historical record.", EventCategory.HistoricalAccuracy, 45, 8),
        new("evt_p09", "Technology Milestone Reached", "Researchers report a significant milestone in the current development program.", EventCategory.Technology, 50, 9),
        new("evt_p10", "Election Cycle Approaching", "Electoral officials have announced the upcoming election schedule.", EventCategory.Election, 60, 10),
        new("evt_p11", "Religious Community Meeting", "Leaders of major religious communities have convened for a national gathering.", EventCategory.Religion, 25, 11),
        new("evt_p12", "Border Incident Reported", "A minor incident along a border region has been reported. Situation is monitored.", EventCategory.Foreign, 55, 12),
        new("evt_p13", "Trade Agreement Signed", "A new trade agreement has been formalized with a partner nation.", EventCategory.Economy, 65, 13),
        new("evt_p14", "Infrastructure Project Launched", "A major infrastructure development project has officially begun.", EventCategory.Domestic, 40, 14),
        new("evt_p15", "Diplomatic Tension Rising", "Diplomatic relations with a neighboring state have become strained.", EventCategory.Foreign, 70, 15),
        new("evt_p16", "Protest Movement Emerges", "A significant protest movement has formed in response to recent policy decisions.", EventCategory.Domestic, 60, 16),
        new("evt_p17", "Natural Disaster Response", "Emergency services have been deployed in response to a natural event.", EventCategory.General, 75, 17),
        new("evt_p18", "Spy Network Exposed", "A foreign intelligence operation within the nation's borders has been uncovered.", EventCategory.Intelligence, 80, 18),
        new("evt_p19", "Population Growth Surge", "The latest demographic data shows unexpected population growth this quarter.", EventCategory.Population, 30, 19),
        new("evt_p20", "Critical Strategic Decision", "A situation requiring immediate strategic attention has emerged.", EventCategory.System, 90, 0)
    ];

    public string SourceSystemId => "PlaceholderEventSource";

    public IReadOnlyList<GameEvent> GenerateEvents(EventContext context)
    {
        var modulo = context.TurnNumber % CycleLength;
        foreach (var template in Templates)
        {
            if (template.CyclePosition != modulo)
            {
                continue;
            }

            return
            [
                new GameEvent
                {
                    EventId = template.EventId,
                    SourceSystemId = SourceSystemId,
                    Title = template.Title,
                    Description = template.Description,
                    Category = template.Category,
                    ImportanceScore = template.ImportanceScore,
                    Effects = new Dictionary<string, string>
                    {
                        ["placeholder"] = "true"
                    },
                    GameDate = context.GameDate,
                    TurnNumber = context.TurnNumber,
                    TargetNationId = context.ActiveNationId,
                    RealTimestamp = DateTime.UtcNow
                }
            ];
        }

        return [];
    }
}
