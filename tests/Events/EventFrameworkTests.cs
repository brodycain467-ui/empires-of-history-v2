using System;
using System.Collections.Generic;
using System.Text.Json;
using EmpiresOfHistoryV2.Events;
using EmpiresOfHistoryV2.Events.Definitions;
using Xunit;

namespace EmpiresOfHistoryV2.Tests.Events;

public class EventFrameworkTests
{
    // ---------------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------------

    private static GameEvent MakeEvent(
        string eventId,
        EventCategory category = EventCategory.General,
        int importanceScore = 10,
        int turnNumber = 1,
        string? nationId = "nation_a",
        string? provinceId = null,
        bool isRead = false)
    {
        return new GameEvent
        {
            EventId = eventId,
            SourceSystemId = "test",
            Title = eventId,
            Description = string.Empty,
            Category = category,
            ImportanceScore = importanceScore,
            TurnNumber = turnNumber,
            TargetNationId = nationId,
            TargetProvinceId = provinceId,
            GameDate = DateTime.UtcNow,
            IsRead = isRead
        };
    }

    // ---------------------------------------------------------------------------
    // EventHistoryLog — GetById O(1) lookup
    // ---------------------------------------------------------------------------

    [Fact]
    public void HistoryLog_GetById_ReturnsCorrectEvent()
    {
        var log = new EventHistoryLog();
        var evt = MakeEvent("evt_001");
        log.Record(evt);

        var result = log.GetById("evt_001");

        Assert.NotNull(result);
        Assert.Equal("evt_001", result.EventId);
    }

    [Fact]
    public void HistoryLog_GetById_ReturnsNull_WhenNotFound()
    {
        var log = new EventHistoryLog();

        var result = log.GetById("nonexistent");

        Assert.Null(result);
    }

    // ---------------------------------------------------------------------------
    // EventHistoryLog — GetByCategory
    // ---------------------------------------------------------------------------

    [Fact]
    public void HistoryLog_GetByCategory_ReturnsOnlyMatchingCategory()
    {
        var log = new EventHistoryLog();
        log.Record(MakeEvent("evt_dom", EventCategory.Domestic));
        log.Record(MakeEvent("evt_for", EventCategory.Foreign));
        log.Record(MakeEvent("evt_dom2", EventCategory.Domestic));

        var results = log.GetByCategory(EventCategory.Domestic);

        Assert.Equal(2, results.Count);
        Assert.All(results, e => Assert.Equal(EventCategory.Domestic, e.Category));
    }

    [Fact]
    public void HistoryLog_GetByCategory_ReturnsEmpty_WhenNoneMatch()
    {
        var log = new EventHistoryLog();
        log.Record(MakeEvent("evt_gen", EventCategory.General));

        var results = log.GetByCategory(EventCategory.War);

        Assert.Empty(results);
    }

    // ---------------------------------------------------------------------------
    // EventHistoryLog — GetByTurn
    // ---------------------------------------------------------------------------

    [Fact]
    public void HistoryLog_GetByTurn_ReturnsOnlyMatchingTurn()
    {
        var log = new EventHistoryLog();
        log.Record(MakeEvent("evt_t1a", turnNumber: 1));
        log.Record(MakeEvent("evt_t2", turnNumber: 2));
        log.Record(MakeEvent("evt_t1b", turnNumber: 1));

        var results = log.GetByTurn(1);

        Assert.Equal(2, results.Count);
        Assert.All(results, e => Assert.Equal(1, e.TurnNumber));
    }

    // ---------------------------------------------------------------------------
    // EventHistoryLog — GetByNation (nation + global)
    // ---------------------------------------------------------------------------

    [Fact]
    public void HistoryLog_GetByNation_ReturnsNationAndGlobalEvents()
    {
        var log = new EventHistoryLog();
        var nationEvent = MakeEvent("evt_nat", nationId: "nation_a");
        var globalEvent = MakeEvent("evt_global", nationId: null); // null → stored as __global__
        var otherEvent = MakeEvent("evt_other", nationId: "nation_b");

        log.Record(nationEvent);
        log.Record(globalEvent);
        log.Record(otherEvent);

        var results = log.GetByNation("nation_a");

        Assert.Contains(results, e => e.EventId == "evt_nat");
        Assert.Contains(results, e => e.EventId == "evt_global");
        Assert.DoesNotContain(results, e => e.EventId == "evt_other");
    }

    // ---------------------------------------------------------------------------
    // EventHistoryLog — max history cap (1000 entries)
    // ---------------------------------------------------------------------------

    [Fact]
    public void HistoryLog_MaxHistoryCap_DropsOldestEvents()
    {
        var log = new EventHistoryLog();

        for (int i = 0; i < 1001; i++)
        {
            log.Record(MakeEvent($"evt_{i:D4}"));
        }

        // Count is capped at 1000
        Assert.Equal(1000, log.Count);
        // Oldest event (evt_0000) should have been evicted from both list and indexes
        Assert.DoesNotContain(log.All, e => e.EventId == "evt_0000");
        Assert.Null(log.GetById("evt_0000"));
        // Newest event should still be present
        Assert.NotNull(log.GetById("evt_1000"));
    }

    // ---------------------------------------------------------------------------
    // EventHistoryLog — MarkAllRead
    // ---------------------------------------------------------------------------

    [Fact]
    public void HistoryLog_MarkAllRead_SetsAllEventsAsRead()
    {
        var log = new EventHistoryLog();
        log.Record(MakeEvent("evt_a", isRead: false));
        log.Record(MakeEvent("evt_b", isRead: false));

        log.MarkAllRead();

        Assert.All(log.All, e => Assert.True(e.IsRead));
    }

    // ---------------------------------------------------------------------------
    // EventChainTracker — HasBeenRaised
    // ---------------------------------------------------------------------------

    [Fact]
    public void ChainTracker_HasBeenRaised_ReturnsFalseBeforeRecord()
    {
        var tracker = new EventChainTracker();

        Assert.False(tracker.HasBeenRaised("evt_test"));
    }

    [Fact]
    public void ChainTracker_HasBeenRaised_ReturnsTrueAfterRecord()
    {
        var tracker = new EventChainTracker();

        tracker.RecordRaised("evt_test", turnNumber: 1);

        Assert.True(tracker.HasBeenRaised("evt_test"));
    }

    // ---------------------------------------------------------------------------
    // EventChainTracker — IsExpired
    // ---------------------------------------------------------------------------

    [Fact]
    public void ChainTracker_IsExpired_ReturnsFalseBeforeExpiryTurn()
    {
        var tracker = new EventChainTracker();
        tracker.RecordRaised("evt_exp", turnNumber: 1, expiresAfterTurns: 5);

        // Expires at turn 6 (1 + 5), current turn is 5
        Assert.False(tracker.IsExpired("evt_exp", currentTurn: 5));
    }

    [Fact]
    public void ChainTracker_IsExpired_ReturnsTrueAfterExpiryTurn()
    {
        var tracker = new EventChainTracker();
        tracker.RecordRaised("evt_exp", turnNumber: 1, expiresAfterTurns: 5);

        // Expires at turn 6 (1 + 5), current turn is 7
        Assert.True(tracker.IsExpired("evt_exp", currentTurn: 7));
    }

    // ---------------------------------------------------------------------------
    // EventChainTracker — PrerequisitesMet
    // ---------------------------------------------------------------------------

    [Fact]
    public void ChainTracker_PrerequisitesMet_ReturnsFalseWhenPrereqNotRaised()
    {
        var tracker = new EventChainTracker();

        var met = tracker.PrerequisitesMet(["evt_prereq"], currentTurn: 1);

        Assert.False(met);
    }

    [Fact]
    public void ChainTracker_PrerequisitesMet_ReturnsTrueWhenPrereqRaised()
    {
        var tracker = new EventChainTracker();
        tracker.RecordRaised("evt_prereq", turnNumber: 1);

        var met = tracker.PrerequisitesMet(["evt_prereq"], currentTurn: 2);

        Assert.True(met);
    }

    // ---------------------------------------------------------------------------
    // EventQueue — FlushSorted descending by ImportanceScore
    // ---------------------------------------------------------------------------

    [Fact]
    public void EventQueue_FlushSorted_ReturnsSortedByImportanceDescending()
    {
        var queue = new EventQueue();
        queue.Enqueue(MakeEvent("evt_low", importanceScore: 10));
        queue.Enqueue(MakeEvent("evt_high", importanceScore: 90));
        queue.Enqueue(MakeEvent("evt_mid", importanceScore: 50));

        var sorted = queue.FlushSorted();

        Assert.Equal(3, sorted.Count);
        Assert.Equal("evt_high", sorted[0].EventId);
        Assert.Equal("evt_mid", sorted[1].EventId);
        Assert.Equal("evt_low", sorted[2].EventId);
    }

    // ---------------------------------------------------------------------------
    // GameEvent — Importance tier mapping
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(1, EventImportance.Minor)]
    [InlineData(25, EventImportance.Minor)]
    [InlineData(26, EventImportance.Moderate)]
    [InlineData(50, EventImportance.Moderate)]
    [InlineData(51, EventImportance.Major)]
    [InlineData(75, EventImportance.Major)]
    [InlineData(76, EventImportance.Critical)]
    [InlineData(100, EventImportance.Critical)]
    public void GameEvent_Importance_MapsScoreToCorrectTier(int score, EventImportance expected)
    {
        var evt = MakeEvent("evt_imp", importanceScore: score);

        Assert.Equal(expected, evt.Importance);
    }

    // ---------------------------------------------------------------------------
    // EventDefinition — JSON deserialization
    // ---------------------------------------------------------------------------

    [Fact]
    public void EventDefinition_DeserializesCorrectlyFromJson()
    {
        const string json = """
        {
          "schema_version": "1.0.0",
          "events": [
            {
              "event_id": "evt_test_01",
              "title": "Test Event",
              "description": "A test event description.",
              "category": "Domestic",
              "importance_score": 42,
              "base_chance": 0.75,
              "historical_weight": 0.3,
              "is_global": true,
              "repeatable": false,
              "requires_player_choice": true,
              "target_nation_id": "nation_x",
              "target_province_id": "prov_y",
              "effects": { "stability": "-1" },
              "prerequisites": ["cond_a"],
              "follow_up_events": ["evt_follow"],
              "parent_event_id": "evt_parent",
              "child_event_ids": ["evt_child"],
              "prerequisite_event_ids": ["evt_prereq"],
              "expires_after_turns": 10,
              "cycle_position": 5,
              "actions": [
                {
                  "action_id": "act_01",
                  "label": "Accept",
                  "action_type": "Respond",
                  "probability_modifiers": { "evt_follow": 1.5 },
                  "follow_up_event_id": "evt_follow"
                }
              ]
            }
          ]
        }
        """;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var db = JsonSerializer.Deserialize<EventActionsDatabase>(json, options);

        Assert.NotNull(db);
        Assert.Equal("1.0.0", db.SchemaVersion);
        Assert.Single(db.Events);

        var def = db.Events[0];
        Assert.Equal("evt_test_01", def.EventId);
        Assert.Equal("Test Event", def.Title);
        Assert.Equal("A test event description.", def.Description);
        Assert.Equal("Domestic", def.Category);
        Assert.Equal(42, def.ImportanceScore);
        Assert.Equal(0.75, def.BaseChance);
        Assert.Equal(0.3, def.HistoricalWeight);
        Assert.True(def.IsGlobal);
        Assert.False(def.Repeatable);
        Assert.True(def.RequiresPlayerChoice);
        Assert.Equal("nation_x", def.TargetNationId);
        Assert.Equal("prov_y", def.TargetProvinceId);
        Assert.Equal("-1", def.Effects["stability"]);
        Assert.Contains("cond_a", def.Prerequisites);
        Assert.Contains("evt_follow", def.FollowUpEvents);
        Assert.Equal("evt_parent", def.ParentEventId);
        Assert.Contains("evt_child", def.ChildEventIds);
        Assert.Contains("evt_prereq", def.PrerequisiteEventIds);
        Assert.Equal(10, def.ExpiresAfterTurns);
        Assert.Equal(5, def.CyclePosition);

        Assert.Single(def.Actions);
        var action = def.Actions[0];
        Assert.Equal("act_01", action.ActionId);
        Assert.Equal("Accept", action.Label);
        Assert.Equal("Respond", action.ActionType);
        Assert.Equal(1.5, action.ProbabilityModifiers["evt_follow"]);
        Assert.Equal("evt_follow", action.FollowUpEventId);
    }
}
