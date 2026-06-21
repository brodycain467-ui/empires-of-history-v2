using System;
using Godot;
using EmpiresOfHistoryV2.Core;

namespace EmpiresOfHistoryV2.UI;

public partial class TurnControls : HBoxContainer
{
    private Label _dateLabel = null!;
    private Button _advanceTurnBtn = null!;
    private Button _saveBtn = null!;
    private Label _toastLabel = null!;
    private Timer _toastTimer = null!;
    private SaveSystem _saveSystem = new();

    public override void _Ready()
    {
        _dateLabel = GetNode<Label>("%DateLabel");
        _advanceTurnBtn = GetNode<Button>("%AdvanceTurnBtn");
        _saveBtn = GetNode<Button>("%SaveBtn");
        _toastLabel = GetNode<Label>("%ToastLabel");
        _toastTimer = GetNode<Timer>("%ToastTimer");

        _advanceTurnBtn.Pressed += () => TurnSystem.Instance.AdvanceTurn();
        _saveBtn.Pressed += SaveToSlotOne;
        _toastTimer.Timeout += () => _toastLabel.Visible = false;

        TurnSystem.Instance.TurnAdvanced += OnTurnAdvanced;
        UpdateDateLabel(GameManager.Instance.GameState.CurrentTurn, GameManager.Instance.GameState.CurrentDate);
    }

    public override void _ExitTree()
    {
        if (TurnSystem.Instance != null)
        {
            TurnSystem.Instance.TurnAdvanced -= OnTurnAdvanced;
        }
    }

    public void Configure(TurnSystem _, SaveSystem saveSystem)
    {
        _saveSystem = saveSystem;
    }

    public void Refresh()
    {
        var state = GameManager.Instance.GameState;
        UpdateDateLabel(state.CurrentTurn, state.CurrentDate);
    }

    private void SaveToSlotOne()
    {
        _saveSystem.Save(1, GameManager.Instance.GameState);
        _toastLabel.Text = "Saved!";
        _toastLabel.Visible = true;
        _toastTimer.Start(2.0);
    }

    private void OnTurnAdvanced(int turn, DateTime date)
    {
        UpdateDateLabel(turn, date);
    }

    private void UpdateDateLabel(int turn, DateTime date)
    {
        _dateLabel.Text = $"{date:MMMM d, yyyy} — Turn {turn}";
    }
}
