using System;
using Godot;
using EmpiresOfHistoryV2.Events;

namespace EmpiresOfHistoryV2.Core;

public partial class TurnSystem : Node
{
    public static TurnSystem Instance { get; private set; } = null!;

    public event Action<int, DateTime>? TurnAdvanced;

    public override void _Ready() => Instance = this;

    public void AdvanceTurn()
    {
        var gm = GameManager.Instance;

        // 1. Advance date
        gm.GameState.AdvanceTurn(months: 3);
        var state = gm.GameState;

        // 2. Reset dirty regions
        gm.DirtyTracker.Reset();

        // 3. Build context (single allocation per turn)
        var simContext = gm.BuildSimulationContext();

        // 4. Tick simulation systems in TickOrder (Timeline=100, Population=300, Economy=400...)
        //    Events (order=200) are handled by EventSystem below, not SimulationManager
        gm.SimulationManager.Tick(simContext);

        // 5. Process events (order=200 in canonical turn, but owned by EventSystem)
        var eventContext = new EventContext
        {
            TurnNumber = state.CurrentTurn,
            GameDate = state.CurrentDate,
            ActiveNationId = state.SelectedNationId,
            AllNations = gm.ContentDatabase.GetAllNations(),
            ActiveNationProvinceIds = state.SelectedNationId != null
                ? gm.OwnershipSystem.GetProvinces(state.SelectedNationId)
                : [],
            TotalEventCount = gm.EventSystem.History.Count
        };
        gm.EventSystem.ProcessTurn(eventContext);

        // 6. Fire TurnAdvanced → triggers UI Refresh (last step)
        TurnAdvanced?.Invoke(state.CurrentTurn, state.CurrentDate);
    }
}
