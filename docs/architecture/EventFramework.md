# Event Framework — Architecture Reference

## Overview

The Event Framework is a data-driven, cycle-based system that generates, queues, filters, and resolves in-game events each turn. It is owned entirely by `GameManager` through the `EventSystem` facade.

---

## Class Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                          GameManager                            │
│  + EventSystem : EventSystem                                    │
└────────────────────────────┬────────────────────────────────────┘
                             │ owns
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                          EventSystem                            │
│  + Queue        : EventQueue                                    │
│  + History      : EventHistoryLog                               │
│  + Resolver     : EventResolver                                 │
│  + ChainTracker : EventChainTracker                             │
│  + ProcessTurn(context) : IReadOnlyList<GameEvent>              │
│  + InjectEvent(gameEvent)                                       │
│  + ResolveAction(eventId, actionId) : EventActionResult         │
│  event TurnEventsResolved                                       │
│  event ActionResolved                                           │
└──────┬────────────┬────────────────┬───────────────────────────┘
       │            │                │
       ▼            ▼                ▼
┌──────────┐ ┌─────────────┐ ┌──────────────────┐
│EventQueue│ │EventHistory │ │  EventResolver   │
│          │ │Log          │ │                  │
│ Enqueue()│ │             │ │ RegisterSource() │
│ Flush-   │ │ Record()    │ │ RegisterHandler()│
│ Sorted() │ │ GetById()   │ │ Resolve()        │
│          │ │ GetByTurn() │ └────────┬─────────┘
└──────────┘ │ GetByNation │         │ calls
             │ GetByCat()  │    ┌────┴──────────┐
             │ GetUnread() │    │  IEventSource │◄──── JsonEventSource
             └─────────────┘    │  IEventHandler│◄──── (future systems)
                                └───────────────┘

┌──────────────────────────────────────────────────────────────┐
│                       JsonEventSource                        │
│  - _definitions : Dictionary<string, EventDefinition>       │
│  - _chainTracker : EventChainTracker                         │
│  + GenerateEvents(context) : IReadOnlyList<GameEvent>        │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│                   EventDefinitionLoader                      │
│  + LoadAll() : Dictionary<string, EventDefinition>          │
│  (reads res://data/events/events.json via File.ReadAllText)  │
└──────────────────────────────────────────────────────────────┘

┌───────────────────┐     ┌────────────────────────┐
│  EventDefinition  │     │  EventActionDefinition │
│  (JSON schema)    │────▶│  (nested in actions[]) │
└───────────────────┘     └────────────────────────┘

┌───────────────────┐     ┌────────────────────────┐
│    GameEvent      │     │      EventAction       │
│  (runtime record) │────▶│  (runtime, from def)   │
└───────────────────┘     └────────────────────────┘

┌────────────────────────┐     ┌────────────────────────┐
│   EventChainTracker    │     │    EventActionResult   │
│  HasBeenRaised()       │     │  returned by           │
│  IsExpired()           │     │  EventSystem.Resolve-  │
│  PrerequisitesMet()    │     │  Action()              │
└────────────────────────┘     └────────────────────────┘
```

---

## Data Flow Per Turn

```
TurnSystem.AdvanceTurn()
  └─► GameManager dispatches EventContext
        └─► EventSystem.ProcessTurn(context)
              ├─► EventResolver.Resolve(context, Queue)
              │     └─► foreach IEventSource:
              │           └─► JsonEventSource.GenerateEvents(context)
              │                 ├── cycle check (turn % 20 == cycle_position)
              │                 ├── prerequisite check (EventChainTracker)
              │                 └─► builds GameEvent[] → Queue.EnqueueRange()
              │     └─► Queue.FlushSorted()  (sorted by ImportanceScore desc)
              │     └─► foreach IEventHandler: handler.Handle(event)
              ├─► EventHistoryLog.RecordRange(resolved)
              └─► TurnEventsResolved?.Invoke(resolved)
                    └─► UI: EventFeedPanel, EventPopupWindow
```

---

## Registering a New Event Source

Any game system that generates events implements `IEventSource`:

```csharp
public class EconomyEventSource : IEventSource
{
    public string SourceSystemId => "EconomySystem";

    public IReadOnlyList<GameEvent> GenerateEvents(EventContext context)
    {
        var results = new List<GameEvent>();
        // Check economic conditions on context, emit GameEvent instances
        return results;
    }
}
```

Register in `GameManager._Ready()`:

```csharp
EventSystem.RegisterSource(new EconomyEventSource(economySystem));
```

---

## Registering a New Event Handler

Any system that reacts to events implements `IEventHandler`:

```csharp
public class NotificationHandler : IEventHandler
{
    public string HandlerId => "NotificationSystem";

    public bool CanHandle(GameEvent gameEvent) =>
        gameEvent.Importance >= EventImportance.Major;

    public void Handle(GameEvent gameEvent)
    {
        // Push to notification queue, play sound, etc.
    }
}
```

---

## Sample Event Chain JSON (3-Stage Election)

```json
{
  "event_id": "evt_election_01",
  "title": "Election Called",
  "description": "The government has called a general election.",
  "category": "Election",
  "importance_score": 60,
  "cycle_position": -1,
  "repeatable": false,
  "requires_player_choice": true,
  "child_event_ids": ["evt_election_02"],
  "actions": [
    {
      "action_id": "act_campaign",
      "label": "Launch Campaign",
      "action_type": "Respond",
      "follow_up_event_id": "evt_election_02"
    }
  ]
},
{
  "event_id": "evt_election_02",
  "title": "Election Campaigning",
  "description": "Parties are actively campaigning across the nation.",
  "category": "Election",
  "importance_score": 50,
  "cycle_position": -1,
  "repeatable": false,
  "parent_event_id": "evt_election_01",
  "prerequisite_event_ids": ["evt_election_01"],
  "child_event_ids": ["evt_election_03"],
  "expires_after_turns": 5
},
{
  "event_id": "evt_election_03",
  "title": "Election Results",
  "description": "The votes have been counted. Results are in.",
  "category": "Election",
  "importance_score": 80,
  "cycle_position": -1,
  "repeatable": false,
  "parent_event_id": "evt_election_02",
  "prerequisite_event_ids": ["evt_election_02"]
}
```

---

## Performance Notes

| Operation | Complexity | Implementation |
|---|---|---|
| `GetById(id)` | **O(1)** | `Dictionary<string, GameEvent>` |
| `GetByCategory(cat)` | **O(1)** | `Dictionary<EventCategory, List<GameEvent>>` |
| `GetByTurn(turn)` | **O(1)** | `Dictionary<int, List<GameEvent>>` |
| `GetByNation(id)` | **O(1)** | `Dictionary<string, List<GameEvent>>` with `__global__` key |
| `GetByProvince(id)` | O(n) scan | Rare query — linear scan acceptable |
| `GetUnread()` | O(n) scan | Acceptable; unread set is small in practice |
| `GetByImportance()` | O(n) scan | Acceptable; called infrequently |
| `FlushSorted()` | O(k log k) | k = events in queue (≤ source count per turn) |
| `EventChainTracker` lookups | **O(1)** | `Dictionary<string, int>` |

The `MaxHistory` cap of 1000 events bounds memory. The `_byId` dictionary may retain references to evicted events (by design — O(1) reverse lookup at the cost of minor memory overhead).

---

## File Map

```
src/Events/
  GameEvent.cs                  — Core event record (pure C#, no Godot)
  EventHistoryLog.cs            — O(1) indexed log with 4 lookup dimensions
  EventSystem.cs                — Top-level facade (owned by GameManager)
  EventQueue.cs                 — Thread-safe FIFO queue
  EventResolver.cs              — Runs sources + dispatches to handlers
  EventChainTracker.cs          — Prerequisite and expiry tracking
  EventAction.cs                — Runtime player action record
  EventActionResult.cs          — Result returned from ResolveAction()
  JsonEventSource.cs            — JSON-driven IEventSource implementation
  PlaceholderEventSource.cs     — Kept for reference/testing
  IEventSource.cs               — Producer interface
  IEventHandler.cs              — Consumer interface
  EventCategory.cs              — Category enum
  EventImportance.cs            — Importance tier enum + score mapping
  EventContext.cs               — Turn snapshot passed to sources
  Definitions/
    EventDefinition.cs          — Deserialized JSON model (snake_case)
    EventActionDefinition.cs    — Nested action model
    EventDefinitionLoader.cs    — Loads events.json via File.ReadAllText

data/events/
  events.json                   — 20 placeholder events (modder-editable)
  README.md                     — Schema reference for modders

tests/Events/
  EventFrameworkTests.cs        — 24 xUnit tests covering all subsystems
```
