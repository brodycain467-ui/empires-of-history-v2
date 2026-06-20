using System.Collections.Generic;
using System.Linq;
using Godot;
using EmpiresOfHistoryV2.Core;
using EmpiresOfHistoryV2.Map.Models;
using EmpiresOfHistoryV2.UI.Theme;

namespace EmpiresOfHistoryV2.UI;

public partial class NationSelectScreen : Control
{
    private VBoxContainer _nationList = null!;
    private string _selectedNationId = string.Empty;
    private readonly Dictionary<string, PanelContainer> _rows = new();

    public override void _Ready()
    {
        Theme = EmpiresDarkTheme.Create();

        _nationList = GetNode<VBoxContainer>("%NationList");
        GetNode<Button>("%BackBtn").Pressed += () => SceneRouter.Instance.GoToMainMenu();
        GetNode<Button>("%ConfirmBtn").Pressed += ConfirmSelection;

        BuildRows();
    }

    private void BuildRows()
    {
        foreach (var child in _nationList.GetChildren())
        {
            child.QueueFree();
        }

        var nations = GameManager.Instance.ContentDatabase.GetAllNations().OrderBy(n => n.DisplayName).ToList();
        if (nations.Count == 0)
        {
            return;
        }

        foreach (var nation in nations)
        {
            _nationList.AddChild(CreateNationRow(nation));
        }

        SelectNation(nations[0].Id);
    }

    private Control CreateNationRow(NationData nation)
    {
        var panel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(0f, 64f),
            MouseFilter = MouseFilterEnum.Stop
        };

        var row = new HBoxContainer();
        panel.AddChild(row);

        row.AddChild(new ColorRect
        {
            Color = Color.FromHtml(nation.Color),
            CustomMinimumSize = new Vector2(40f, 40f)
        });

        row.AddChild(new Label
        {
            Text = nation.DisplayName,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        });

        row.AddChild(new Label { Text = FormatTitle(nation.Tier) });
        row.AddChild(new Label { Text = FormatTitle(nation.GovernmentType) });

        panel.GuiInput += @event =>
        {
            if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
            {
                SelectNation(nation.Id);
            }
        };

        panel.MouseEntered += () =>
        {
            if (nation.Id != _selectedNationId)
            {
                panel.AddThemeStyleboxOverride("panel", CreateRowStyle("#3a2f1e", "#c9a84c"));
            }
        };
        panel.MouseExited += () =>
        {
            if (nation.Id != _selectedNationId)
            {
                panel.AddThemeStyleboxOverride("panel", CreateRowStyle("#2a1f0e", "#7a5d2c"));
            }
        };

        panel.AddThemeStyleboxOverride("panel", CreateRowStyle("#2a1f0e", "#7a5d2c"));
        _rows[nation.Id] = panel;
        return panel;
    }

    private void SelectNation(string nationId)
    {
        _selectedNationId = nationId;
        foreach (var row in _rows)
        {
            row.Value.AddThemeStyleboxOverride(
                "panel",
                row.Key == nationId ? CreateRowStyle("#2a1f0e", "#c9a84c", 2) : CreateRowStyle("#2a1f0e", "#7a5d2c"));
        }
    }

    private void ConfirmSelection()
    {
        if (string.IsNullOrWhiteSpace(_selectedNationId))
        {
            return;
        }

        GameManager.Instance.NewGame(_selectedNationId);
        SceneRouter.Instance.GoToWorldMap();
    }

    private static StyleBoxFlat CreateRowStyle(string background, string border, int borderSize = 1)
    {
        return new StyleBoxFlat
        {
            BgColor = Color.FromHtml(background),
            BorderColor = Color.FromHtml(border),
            BorderWidthTop = borderSize,
            BorderWidthRight = borderSize,
            BorderWidthBottom = borderSize,
            BorderWidthLeft = borderSize,
            ContentMarginTop = 8,
            ContentMarginBottom = 8,
            ContentMarginLeft = 8,
            ContentMarginRight = 8
        };
    }

    private static string FormatTitle(string value)
    {
        var parts = value.Replace('_', ' ').Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < parts.Length; i++)
        {
            parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i][1..];
        }

        return string.Join(' ', parts);
    }
}
