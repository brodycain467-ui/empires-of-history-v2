using System;
using System.Linq;
using Godot;
using EmpiresOfHistoryV2.Events;
using EmpiresOfHistoryV2.UI.Theme;

namespace EmpiresOfHistoryV2.UI.Events;

public partial class EventFeedPanel : PanelContainer
{
    public event Action<GameEvent>? EventSelected;

    private VBoxContainer _rows = null!;
    public override void _Ready()
    {
        Theme = EmpiresDarkTheme.Create();
        AddThemeStyleboxOverride("panel", CreatePanelStyle());

        var margin = new MarginContainer();
        margin.AddThemeConstantOverride("margin_left", 10);
        margin.AddThemeConstantOverride("margin_top", 10);
        margin.AddThemeConstantOverride("margin_right", 10);
        margin.AddThemeConstantOverride("margin_bottom", 10);
        AddChild(margin);

        var root = new VBoxContainer();
        root.AddThemeConstantOverride("separation", 8);
        margin.AddChild(root);

        root.AddChild(new Label
        {
            Text = "EVENT FEED",
            ThemeTypeVariation = "Label"
        });

        var scroll = new ScrollContainer
        {
            CustomMinimumSize = new Vector2(0f, 160f),
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddChild(scroll);

        _rows = new VBoxContainer();
        _rows.AddThemeConstantOverride("separation", 6);
        scroll.AddChild(_rows);

    }

    public void RefreshFeed(EventHistoryLog history)
    {
        if (_rows == null)
        {
            return;
        }

        foreach (var child in _rows.GetChildren())
        {
            child.QueueFree();
        }

        var events = history.All.Take(5).ToList();
        if (events.Count == 0)
        {
            _rows.AddChild(CreateEmptyStateLabel());
            return;
        }

        foreach (var gameEvent in events)
        {
            _rows.AddChild(CreateRow(gameEvent));
        }
    }

    private Control CreateRow(GameEvent gameEvent)
    {
        var panel = new PanelContainer
        {
            MouseFilter = MouseFilterEnum.Stop,
            FocusMode = FocusModeEnum.All
        };

        panel.AddThemeStyleboxOverride("panel", CreateRowStyle(false, gameEvent.Importance == EventImportance.Critical));

        var row = new HBoxContainer();
        row.AddThemeConstantOverride("separation", 8);
        panel.AddChild(row);

        row.AddChild(new ColorRect
        {
            Color = GetImportanceColor(gameEvent.Importance),
            CustomMinimumSize = new Vector2(10f, 10f)
        });

        row.AddChild(new Label
        {
            Text = gameEvent.Category.ToString().ToUpperInvariant(),
            CustomMinimumSize = new Vector2(110f, 0f),
            Modulate = Color.FromHtml("#c9a84c")
        });

        row.AddChild(new Label
        {
            Text = gameEvent.Title,
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        });

        panel.GuiInput += @event =>
        {
            if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
            {
                EventSelected?.Invoke(gameEvent);
            }
        };

        panel.MouseEntered += () => panel.AddThemeStyleboxOverride("panel", CreateRowStyle(true, gameEvent.Importance == EventImportance.Critical));
        panel.MouseExited += () => panel.AddThemeStyleboxOverride("panel", CreateRowStyle(false, gameEvent.Importance == EventImportance.Critical));

        if (gameEvent.Importance == EventImportance.Critical)
        {
            var tween = CreateTween();
            tween.SetLoops();
            tween.TweenProperty(panel, "self_modulate", new Color(1f, 1f, 1f, 0.82f), 0.7);
            tween.TweenProperty(panel, "self_modulate", Colors.White, 0.7);
        }

        return panel;
    }

    private static StyleBoxFlat CreatePanelStyle() => new()
    {
        BgColor = Color.FromHtml("#1a1208d9"),
        BorderColor = Color.FromHtml("#7a5d2c"),
        BorderWidthTop = 1,
        BorderWidthRight = 1,
        BorderWidthBottom = 1,
        BorderWidthLeft = 1
    };

    private static StyleBoxFlat CreateRowStyle(bool hovered, bool critical) => new()
    {
        BgColor = Color.FromHtml(hovered ? "#2a1f0e" : "#21170b"),
        BorderColor = Color.FromHtml(critical ? "#c9a84c" : "#5a4523"),
        BorderWidthTop = critical ? 2 : 1,
        BorderWidthRight = critical ? 2 : 1,
        BorderWidthBottom = critical ? 2 : 1,
        BorderWidthLeft = critical ? 2 : 1,
        ContentMarginTop = 6,
        ContentMarginRight = 8,
        ContentMarginBottom = 6,
        ContentMarginLeft = 8
    };

    private static Color GetImportanceColor(EventImportance importance) => importance switch
    {
        EventImportance.Minor => Color.FromHtml("#a08060"),
        EventImportance.Moderate => Color.FromHtml("#f0e6cc"),
        EventImportance.Major => Color.FromHtml("#c9a84c"),
        EventImportance.Critical => Color.FromHtml("#c9a84c"),
        _ => Color.FromHtml("#a08060")
    };

    private static Label CreateEmptyStateLabel() => new()
    {
        Text = "No events yet.",
        Modulate = Color.FromHtml("#a08060")
    };
}
