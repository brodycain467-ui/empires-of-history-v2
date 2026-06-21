# Event Definitions — Schema Reference

All game events are defined in `events.json`. New events can be added without recompiling.

## Schema

| Field | Type | Required | Description |
|---|---|---|---|
| `event_id` | string | ✅ | Unique identifier, e.g. `evt_dom_001` |
| `title` | string | ✅ | Short display title |
| `description` | string | ✅ | Body text shown in popup |
| `category` | string | ✅ | One of: General, Domestic, Foreign, HistoricalAccuracy, Religion, Election, Technology, Economy, War, Intelligence, GIA, Population, Tutorial, System |
| `importance_score` | int 1–100 | ✅ | 1–25 Minor, 26–50 Moderate, 51–75 Major, 76–100 Critical |
| `base_chance` | float 0–1 | ✅ | Probability this event fires when eligible |
| `historical_weight` | float 0–1 | ✅ | How closely this follows historical record (future HistoricalAccuracy system) |
| `is_global` | bool | ✅ | true = fires for all nations; false = only active nation |
| `repeatable` | bool | ✅ | Whether this event can fire more than once per game |
| `requires_player_choice` | bool | ✅ | Whether popup blocks turn advance until player responds |
| `target_nation_id` | string? | — | Lock event to specific nation ID; null = active nation |
| `target_province_id` | string? | — | Lock event to specific province ID |
| `effects` | object | ✅ | Key/value effects bag; each system reads its own keys |
| `prerequisites` | string[] | ✅ | Arbitrary prerequisite condition tags (future use) |
| `follow_up_events` | string[] | ✅ | Event IDs to queue after this event resolves |
| `parent_event_id` | string? | — | ID of the event that triggered this one in a chain |
| `child_event_ids` | string[] | ✅ | IDs of events in the chain following this one |
| `prerequisite_event_ids` | string[] | ✅ | Event IDs that must have been raised before this fires |
| `expires_after_turns` | int? | — | If set, event chain context expires this many turns after being raised |
| `cycle_position` | int | ✅ | 0–19 = fires on turn % 20 == value; -1 = not cycle-driven |
| `actions` | array | ✅ | Player response options (empty = no choice required) |

## Action Schema

| Field | Type | Description |
|---|---|---|
| `action_id` | string | Unique within event |
| `label` | string | Button text shown to player |
| `action_type` | string | Respond \| Ignore \| Delegate \| ViewDetails |
| `probability_modifiers` | object | event_id → multiplier adjustments for future events |
| `follow_up_event_id` | string? | Injects this event next turn if action taken |

## Adding New Events

1. Add entry to `data/events/events.json`
2. Set a unique `event_id`
3. Choose `category` from the supported list
4. Set `cycle_position: -1` for non-cycle events (triggered by systems)
5. Restart the game — no recompile needed
