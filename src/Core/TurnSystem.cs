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
        GameManager.Instance.GameState.AdvanceTurn(months: 3);
        var state = GameManager.Instance.GameState;

        var context = new EventContext
        {
            TurnNumber = state.CurrentTurn,
            GameDate = state.CurrentDate,
            ActiveNationId = state.SelectedNationId,
            AllNations = GameManager.Instance.ContentDatabase.GetAllNations(),
            ActiveNationProvinceIds = state.SelectedNationId != null
                ? GameManager.Instance.OwnershipSystem.GetProvinces(state.SelectedNationId)
                : [],
            TotalEventCount = GameManager.Instance.EventSystem.History.Count
        };

        GameManager.Instance.EventSystem.ProcessTurn(context);
        TurnAdvanced?.Invoke(state.CurrentTurn, state.CurrentDate);
    }
}
