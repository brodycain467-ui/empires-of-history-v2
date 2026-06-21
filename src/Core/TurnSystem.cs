using System;
using Godot;

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
        TurnAdvanced?.Invoke(state.CurrentTurn, state.CurrentDate);
    }
}
