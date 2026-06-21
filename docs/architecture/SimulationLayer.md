# Simulation Layer Architecture — Empires of History V2

## Overview

Phase 4.75 introduces a permanent simulation engine architecture. Every future gameplay system (Economy, Government, Religion, Military, etc.) plugs into this layer — no modifications to the engine are needed to add a new system.

---

## Repository Tree

```
empires-of-history-v2/
├── data/
│   └── events/               # JSON event definitions
├── docs/
│   └── architecture/
│       └── SimulationLayer.md  ← this file
├── scenes/
│   └── Events/               # Event UI scene files
├── src/
│   ├── Core/
│   │   ├── ContentDatabase.cs
│   │   ├── GameManager.cs      UPDATED — SimulationManager + DirtyTracker + BuildSimulationContext()
│   │   ├── GameState.cs
│   │   ├── SaveSystem.cs
│   │   ├── SceneRouter.cs
│   │   └── TurnSystem.cs       UPDATED — SimulationManager.Tick() in turn order
│   ├── Events/
│   │   ├── Definitions/
│   │   │   ├── EventActionDefinition.cs
│   │   │   ├── EventDefinition.cs
│   │   │   └── EventDefinitionLoader.cs
│   │   ├── EventAction.cs
│   │   ├── EventActionResult.cs
│   │   ├── EventCategory.cs
│   │   ├── EventChainTracker.cs
│   │   ├── EventContext.cs
│   │   ├── EventHistoryLog.cs
│   │   ├── EventImportance.cs
│   │   ├── EventQueue.cs
│   │   ├── EventResolver.cs
│   │   ├── EventSystem.cs
│   │   ├── GameEvent.cs
│   │   ├── IEventHandler.cs
│   │   ├── IEventSource.cs
│   │   ├── JsonEventSource.cs
│   │   └── PlaceholderEventSource.cs
│   ├── Map/
│   │   ├── Models/
│   │   │   ├── NationData.cs
│   │   │   ├── OfficialData.cs
│   │   │   └── ProvinceData.cs
│   │   ├── Rendering/
│   │   │   ├── LODManager.cs
│   │   │   ├── MapCamera.cs
│   │   │   ├── ProvincePool.cs
│   │   │   ├── ProvinceVisual.cs
│   │   │   ├── ViewportCuller.cs
│   │   │   └── WorldMapManager.cs
│   │   └── Systems/
│   │       ├── BorderHistorySystem.cs
│   │       ├── NationColorRegistry.cs
│   │       └── OwnershipSystem.cs
│   ├── Simulation/
│   │   ├── DirtyRegionTracker.cs     NEW — O(1) dirty-region tracking
│   │   ├── ISimulationProvider.cs    NEW — queryable state interface
│   │   ├── ISimulationSerializer.cs  NEW — save/load interface
│   │   ├── ISimulationSystem.cs      NEW — base system contract
│   │   ├── ISimulationTick.cs        NEW — per-turn execution interface
│   │   ├── SimulationContext.cs      NEW — immutable per-turn snapshot
│   │   ├── SimulationManager.cs      NEW — system registry + orchestrator
│   │   ├── SimulationTickOrder.cs    NEW — canonical execution order constants
│   │   └── Systems/
│   │       ├── EconomySystem.cs      NEW stub (Phase 6)
│   │       ├── ElectionsSystem.cs    NEW stub (Phase 7)
│   │       ├── GIAAdvisorSystem.cs   NEW stub (Phase 10)
│   │       ├── GovernmentSystem.cs   NEW stub (Phase 7)
│   │       ├── IntelligenceSystem.cs NEW stub (Phase 9)
│   │       ├── MilitarySystem.cs     NEW stub (Phase 9)
│   │       ├── PopulationSystem.cs   NEW stub (Phase 6)
│   │       ├── ReligionSystem.cs     NEW stub (Phase 8)
│   │       ├── TechnologySystem.cs   NEW stub (Phase 7)
│   │       └── TimelineSystem.cs     NEW stub (Phase 5)
│   └── UI/
│       ├── BottomTabBar.cs
│       ├── Events/
│       │   ├── EventArchiveScreen.cs
│       │   ├── EventFeedPanel.cs
│       │   └── EventPopupWindow.cs
│       ├── MainMenuScreen.cs
│       ├── NationInfoPanel.cs
│       ├── NationSelectScreen.cs
│       ├── SaveLoadDialog.cs
│       ├── Theme/
│       │   └── EmpiresDarkTheme.cs
│       ├── TopBar.cs
│       ├── TurnControls.cs
│       └── WorldMapScreen.cs
└── tests/
    ├── Events/
    │   └── EventFrameworkTests.cs
    ├── Simulation/
    │   └── SimulationManagerTests.cs  NEW — 16 tests
    └── EmpiresOfHistoryV2.Tests.csproj
```

---

## Interface Class Diagram

```
ISimulationSystem
│   SystemId : string
│   TickOrder : int
│   IsEnabled : bool
│   Initialize(SimulationContext)
│   Dispose()
│
├── ISimulationTick
│       Tick(SimulationContext)
│
├── ISimulationSerializer
│       Serialize() : string
│       Deserialize(string)
│
└── ISimulationProvider
        GetValue(string key) : string?
        GetExportedKeys() : IReadOnlyList<string>

All Phase 5–10 system stubs implement all three derived interfaces:
  ISimulationTick + ISimulationSerializer + ISimulationProvider

SimulationManager
│   RegisterSystem(ISimulationSystem)
│   UnregisterSystem(string)
│   GetSystem(string) : ISimulationSystem?
│   GetSystem<T>() : T?
│   AllSystems : IReadOnlyList<ISimulationSystem>
│   Initialize(SimulationContext)
│   Tick(SimulationContext)          → calls ISimulationTick systems in TickOrder
│   Save() : Dictionary<string,string>  → calls ISimulationSerializer systems
│   Load(Dictionary<string,string>)  → calls ISimulationSerializer systems
│   QueryProvider(systemId, key)     → delegates to ISimulationProvider

SimulationContext (immutable init properties)
│   TurnNumber : int
│   GameDate : DateTime
│   ActiveNationId : string?
│   AllNations : IReadOnlyList<NationData>
│   ActiveNationProvinceIds : IReadOnlyList<string>
│   EventHistory : EventHistoryLog
│   InjectEvent : Action<GameEvent>?

DirtyRegionTracker
│   MarkNationDirty(string)
│   MarkProvinceDirty(string)
│   MarkGlobalDirty()
│   IsNationDirty(string) : bool
│   IsProvinceDirty(string) : bool
│   IsGlobalDirty : bool
│   DirtyNations : IReadOnlySet<string>
│   DirtyProvinces : IReadOnlySet<string>
│   Reset()
```

---

## Simulation Flow Diagram

```
Player clicks "End Turn"
         │
         ▼
TurnControls._on_end_turn_pressed()
         │
         ▼
TurnSystem.AdvanceTurn()
         │
         ├─ 1. GameState.AdvanceTurn(months: 3)    — date/turn counter
         │
         ├─ 2. DirtyTracker.Reset()                — clear last turn's dirty flags
         │
         ├─ 3. BuildSimulationContext()             — single allocation, immutable snapshot
         │
         ├─ 4. SimulationManager.Tick(context)      — runs all enabled ISimulationTick systems
         │         (Timeline=100, Population=300, Economy=400, Technology=500,
         │          Government=600, Elections=700, Religion=800,
         │          Intelligence=900, Military=1000, GIAAdvisor=1100)
         │
         ├─ 5. EventSystem.ProcessTurn(eventContext) — event order=200, owned by EventSystem
         │         └─ EventResolver.Resolve()
         │               ├─ IEventSource.GenerateEvents()
         │               └─ IEventHandler.Handle()
         │
         └─ 6. TurnAdvanced?.Invoke()               — UI refresh (EventFeedPanel, TopBar, etc.)
```

---

## Turn Execution Order (SimulationTickOrder)

| Step | Order | System | Status | Phase |
|------|-------|--------|--------|-------|
| 1 | 100 | TimelineSystem | Stub | 5 |
| 2 | 200 | EventSystem | ✅ Active | 4 (owned by TurnSystem, not SimulationManager) |
| 3 | 300 | PopulationSystem | Stub | 6 |
| 4 | 400 | EconomySystem | Stub | 6 |
| 5 | 500 | TechnologySystem | Stub | 7 |
| 6 | 600 | GovernmentSystem | Stub | 7 |
| 7 | 700 | ElectionsSystem | Stub | 7 |
| 8 | 800 | ReligionSystem | Stub | 8 |
| 9 | 900 | IntelligenceSystem | Stub | 9 |
| 10 | 1000 | MilitarySystem | Stub | 9 |
| 11 | 1100 | GIAAdvisorSystem | Stub | 10 |
| — | — | UI Refresh | ✅ Active | 4 (TurnAdvanced event, not a simulation system) |

---

## Performance Notes

### Dirty Region Strategy

- **Target**: 250 nations, 5,000 provinces, 60 FPS
- Systems call `DirtyTracker.IsNationDirty(nationId)` before expensive per-nation calculations
- `IsNationDirty` / `IsProvinceDirty` use `HashSet<string>` — O(1) lookup
- `MarkGlobalDirty()` sets a bool flag — eliminates per-entry iteration on full-world recalc
- `Reset()` called once per turn (beginning of `AdvanceTurn`) — O(n) where n = dirty entries from previous turn, typically << 250

### O(1) Lookup Paths

- `SimulationManager.GetSystem(systemId)` — `Dictionary<string, ISimulationSystem>` — O(1)
- `SimulationManager.QueryProvider(systemId, key)` — O(1) system lookup + O(1) key lookup
- `EventHistoryLog.GetById` — `Dictionary<string, GameEvent>` — O(1)
- `EventHistoryLog.GetByCategory` / `GetByTurn` / `GetByNation` — indexed dictionaries — O(1)

### Single Context Allocation

`BuildSimulationContext()` is called **once per turn** and reused by both `SimulationManager.Tick()` and can be referenced by any system via `SimulationContext` parameters. No per-system allocation.

---

## Dependency Graph

```
Phase 5 — TimelineSystem
  └── reads: SimulationContext (AllNations, TurnNumber, GameDate)
  └── writes: EventSystem.InjectEvent (historical events)

Phase 6 — PopulationSystem
  └── reads: SimulationContext, DirtyRegionTracker
  └── depends on: TimelineSystem (historical population data)

Phase 6 — EconomySystem
  └── reads: SimulationContext, DirtyRegionTracker
  └── depends on: PopulationSystem (labor supply)
  └── depends on: GovernmentSystem (tax policy)

Phase 7 — TechnologySystem
  └── reads: SimulationContext
  └── depends on: EconomySystem (R&D budget)
  └── depends on: PopulationSystem (researchers)

Phase 7 — GovernmentSystem
  └── reads: SimulationContext
  └── depends on: ElectionsSystem (mandate/approval)

Phase 7 — ElectionsSystem
  └── reads: SimulationContext
  └── depends on: GovernmentSystem (current government type)
  └── depends on: EconomySystem (economic conditions)

Phase 8 — ReligionSystem
  └── reads: SimulationContext
  └── depends on: PopulationSystem (demographics)
  └── depends on: GovernmentSystem (state religion policy)

Phase 9 — IntelligenceSystem
  └── reads: SimulationContext, EventHistoryLog
  └── depends on: all other systems via QueryProvider

Phase 9 — MilitarySystem
  └── reads: SimulationContext, DirtyRegionTracker
  └── depends on: EconomySystem (military budget)
  └── depends on: GovernmentSystem (war powers)

Phase 10 — GIAAdvisorSystem
  └── reads: SimulationContext, EventHistoryLog
  └── depends on: all systems via SimulationManager.QueryProvider
```

---

## How to Add a New System

1. **Create the class** in `src/Simulation/Systems/YourSystem.cs`:

```csharp
namespace EmpiresOfHistoryV2.Simulation.Systems;

public class YourSystem : ISimulationTick, ISimulationSerializer, ISimulationProvider
{
    public string SystemId => "YourSystem";
    public int TickOrder => SimulationTickOrder.YourOrder; // add constant to SimulationTickOrder.cs
    public bool IsEnabled => true; // set true when implementing

    public void Initialize(SimulationContext context) { /* setup */ }
    public void Dispose() { /* cleanup */ }

    public void Tick(SimulationContext context)
    {
        // 1. Check dirty regions before expensive work
        var gm = GameManager.Instance;
        foreach (var nation in context.AllNations)
        {
            if (!gm.DirtyTracker.IsNationDirty(nation.Id)) continue;
            // ... do expensive calculation
        }
        // 2. Produce output via InjectEvent, not direct system calls
        context.InjectEvent?.Invoke(new GameEvent { ... });
    }

    public string Serialize() => System.Text.Json.JsonSerializer.Serialize(_state);
    public void Deserialize(string json) => _state = System.Text.Json.JsonSerializer.Deserialize<MyState>(json)!;

    public string? GetValue(string key) => key switch
    {
        "my_key" => _state.MyValue.ToString(),
        _ => null
    };
    public IReadOnlyList<string> GetExportedKeys() => ["my_key"];
}
```

2. **Add a TickOrder constant** to `SimulationTickOrder.cs`:

```csharp
public const int YourSystem = <number between existing constants>;
```

3. **Register in GameManager._Ready()**:

```csharp
SimulationManager.RegisterSystem(new YourSystem());
```

4. **Read cross-system state** via `SimulationManager.QueryProvider` — never hold a direct reference to another system.

5. **Write unit tests** in `tests/Simulation/YourSystemTests.cs` following the pattern in `SimulationManagerTests.cs`.

That's all. No changes to `SimulationManager`, `TurnSystem`, or any other engine file are required.
