using System;
using Godot;
using EmpiresOfHistoryV2.Core;
using EmpiresOfHistoryV2.UI.Theme;

namespace EmpiresOfHistoryV2.UI;

public partial class SaveLoadDialog : Control
{
    public enum DialogMode
    {
        Save,
        Load
    }

    public event Action? SaveDataChanged;
    public event Action? LoadCompleted;

    private Label _headerLabel = null!;
    private VBoxContainer _slotList = null!;
    private readonly SaveSystem _saveSystem = new();
    private DialogMode _mode = DialogMode.Load;

    public override void _Ready()
    {
        Theme = EmpiresDarkTheme.Create();
        Visible = false;

        _headerLabel = GetNode<Label>("%HeaderLabel");
        _slotList = GetNode<VBoxContainer>("%SlotList");
        GetNode<Button>("%CloseBtn").Pressed += HideDialog;
    }

    public void ShowDialog(DialogMode mode)
    {
        _mode = mode;
        _headerLabel.Text = mode == DialogMode.Save ? "SAVE GAME" : "LOAD GAME";
        BuildSlotRows();
        Visible = true;
    }

    public void HideDialog()
    {
        Visible = false;
    }

    private void BuildSlotRows()
    {
        foreach (var child in _slotList.GetChildren())
        {
            child.QueueFree();
        }

        var slots = _saveSystem.GetAllSaveSlots();
        for (var i = 0; i < slots.Count; i++)
        {
            var slot = i + 1;
            var data = slots[i];
            _slotList.AddChild(CreateSlotRow(slot, data));
        }
    }

    private Control CreateSlotRow(int slot, SaveData? data)
    {
        var row = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };

        var label = new Label
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            Text = data == null
                ? $"Slot {slot}: — Empty —"
                : $"Slot {slot}: {data.SaveName} ({data.GameDate})"
        };
        row.AddChild(label);

        var actionBtn = new Button
        {
            Text = _mode == DialogMode.Save ? "SAVE" : "LOAD",
            Disabled = _mode == DialogMode.Load && data == null
        };
        actionBtn.Pressed += () =>
        {
            if (_mode == DialogMode.Save)
            {
                _saveSystem.Save(slot, GameManager.Instance.GameState);
                SaveDataChanged?.Invoke();
                BuildSlotRows();
                return;
            }

            if (_saveSystem.Load(slot) != null)
            {
                LoadCompleted?.Invoke();
                if (GetTree().CurrentScene?.SceneFilePath != SceneRouter.WorldMapScene)
                {
                    SceneRouter.Instance.GoToWorldMap();
                    return;
                }

                HideDialog();
            }
        };
        row.AddChild(actionBtn);

        var deleteBtn = new Button
        {
            Text = "✕",
            Visible = data != null
        };
        deleteBtn.AddThemeColorOverride("font_color", Color.FromHtml("#f0e6cc"));
        deleteBtn.AddThemeStyleboxOverride("normal", CreateDeleteStyle());
        deleteBtn.AddThemeStyleboxOverride("hover", CreateDeleteStyle(Color.FromHtml("#a01a1a")));
        deleteBtn.Pressed += () =>
        {
            _saveSystem.DeleteSave(slot);
            SaveDataChanged?.Invoke();
            BuildSlotRows();
        };
        row.AddChild(deleteBtn);

        return row;
    }

    private static StyleBoxFlat CreateDeleteStyle(Color? color = null)
    {
        var style = new StyleBoxFlat
        {
            BgColor = color ?? Color.FromHtml("#8b1414"),
            BorderColor = Color.FromHtml("#c9a84c"),
            BorderWidthTop = 1,
            BorderWidthRight = 1,
            BorderWidthBottom = 1,
            BorderWidthLeft = 1
        };

        return style;
    }
}
