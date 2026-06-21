using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using EmpiresOfHistoryV2.Events;
using EmpiresOfHistoryV2.UI.Theme;

namespace EmpiresOfHistoryV2.UI.Events;

public partial class EventArchiveScreen : Control
{
    public event Action<GameEvent>? EventSelected;

    private EventHistoryLog? _history;
    private VBoxContainer _eventRows = null!;
    private Label _summaryLabel = null!;
    private EventCategory? _categoryFilter;
    private EventImportance? _importanceFilter;
    private bool _unreadOnly;

    public override void _Ready()
    {
        Theme = EmpiresDarkTheme.Create();
        Visible = false;

        var backdrop = new ColorRect
        {
            LayoutMode = 1,
            AnchorRight = 1f,
            AnchorBottom = 1f,
            Color = new Color(0f, 0f, 0f, 0.6f)
        };
        AddChild(backdrop);

        var margin = new MarginContainer
        {
            LayoutMode = 1,
            AnchorRight = 1f,
            AnchorBottom = 1f
        };
        margin.AddThemeConstantOverride("margin_left", 32);
        margin.AddThemeConstantOverride("margin_top", 32);
        margin.AddThemeConstantOverride("margin_right", 32);
        margin.AddThemeConstantOverride("margin_bottom", 32);
        AddChild(margin);

        var shell = new HBoxContainer();
        shell.AddThemeConstantOverride("separation", 16);
        margin.AddChild(shell);

        var sidebarPanel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(250f, 0f)
        };
        sidebarPanel.AddThemeStyleboxOverride("panel", CreatePanelStyle());
        shell.AddChild(sidebarPanel);

        var sidebarMargin = new MarginContainer();
        sidebarMargin.AddThemeConstantOverride("margin_left", 12);
        sidebarMargin.AddThemeConstantOverride("margin_top", 12);
        sidebarMargin.AddThemeConstantOverride("margin_right", 12);
        sidebarMargin.AddThemeConstantOverride("margin_bottom", 12);
        sidebarPanel.AddChild(sidebarMargin);

        var sidebar = new VBoxContainer();
        sidebar.AddThemeConstantOverride("separation", 6);
        sidebarMargin.AddChild(sidebar);

        sidebar.AddChild(new Label
        {
            Text = "FILTERS",
            Modulate = Color.FromHtml("#c9a84c")
        });

        sidebar.AddChild(CreateFilterButton("ALL EVENTS", () =>
        {
            _categoryFilter = null;
            _importanceFilter = null;
            _unreadOnly = false;
            RefreshRows();
        }));

        sidebar.AddChild(CreateFilterButton("UNREAD", () =>
        {
            _categoryFilter = null;
            _importanceFilter = null;
            _unreadOnly = true;
            RefreshRows();
        }));

        foreach (var importance in Enum.GetValues<EventImportance>())
        {
            var captured = importance;
            sidebar.AddChild(CreateFilterButton(captured.ToString().ToUpperInvariant(), () =>
            {
                _categoryFilter = null;
                _importanceFilter = captured;
                _unreadOnly = false;
                RefreshRows();
            }));
        }

        foreach (var category in Enum.GetValues<EventCategory>())
        {
            var captured = category;
            sidebar.AddChild(CreateFilterButton(captured.ToString().ToUpperInvariant(), () =>
            {
                _categoryFilter = captured;
                _importanceFilter = null;
                _unreadOnly = false;
                RefreshRows();
            }));
        }

        var contentPanel = new PanelContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        contentPanel.AddThemeStyleboxOverride("panel", CreatePanelStyle());
        shell.AddChild(contentPanel);

        var contentMargin = new MarginContainer();
        contentMargin.AddThemeConstantOverride("margin_left", 16);
        contentMargin.AddThemeConstantOverride("margin_top", 16);
        contentMargin.AddThemeConstantOverride("margin_right", 16);
        contentMargin.AddThemeConstantOverride("margin_bottom", 16);
        contentPanel.AddChild(contentMargin);

        var content = new VBoxContainer();
        content.AddThemeConstantOverride("separation", 10);
        contentMargin.AddChild(content);

        var header = new HBoxContainer();
        content.AddChild(header);

        header.AddChild(new Label
        {
            Text = "EVENT ARCHIVE",
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            Modulate = Color.FromHtml("#c9a84c")
        });

        var markAllRead = new Button { Text = "MARK ALL READ" };
        markAllRead.Pressed += () =>
        {
            _history?.MarkAllRead();
            RefreshRows();
        };
        header.AddChild(markAllRead);

        var closeButton = new Button { Text = "CLOSE" };
        closeButton.Pressed += Hide;
        header.AddChild(closeButton);

        _summaryLabel = new Label
        {
            Text = "0 events"
        };
        content.AddChild(_summaryLabel);

        var scroll = new ScrollContainer
        {
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        content.AddChild(scroll);

        _eventRows = new VBoxContainer();
        _eventRows.AddThemeConstantOverride("separation", 8);
        scroll.AddChild(_eventRows);
    }

    public void ShowArchive(EventHistoryLog history)
    {
        _history = history;
        Visible = true;
        RefreshRows();
    }

    public void Refresh(EventHistoryLog history)
    {
        _history = history;
        if (Visible)
        {
            RefreshRows();
        }
    }

    private void RefreshRows()
    {
        if (_eventRows == null || _history == null)
        {
            return;
        }

        foreach (var child in _eventRows.GetChildren())
        {
            child.QueueFree();
        }

        var filtered = ApplyFilters(_history.All).ToList();
        _summaryLabel.Text = filtered.Count == 1 ? "1 event" : $"{filtered.Count} events";

        if (filtered.Count == 0)
        {
            _eventRows.AddChild(new Label
            {
                Text = "No events match the current filter.",
                Modulate = Color.FromHtml("#a08060")
            });
            return;
        }

        foreach (var gameEvent in filtered)
        {
            _eventRows.AddChild(CreateEventRow(gameEvent));
        }
    }

    private IEnumerable<GameEvent> ApplyFilters(IEnumerable<GameEvent> events)
    {
        var filtered = events;
        if (_unreadOnly)
        {
            filtered = filtered.Where(gameEvent => !gameEvent.IsRead);
        }

        if (_importanceFilter.HasValue)
        {
            filtered = filtered.Where(gameEvent => gameEvent.Importance == _importanceFilter.Value);
        }

        if (_categoryFilter.HasValue)
        {
            filtered = filtered.Where(gameEvent => gameEvent.Category == _categoryFilter.Value);
        }

        return filtered;
    }

    private Control CreateEventRow(GameEvent gameEvent)
    {
        var panel = new PanelContainer
        {
            MouseFilter = MouseFilterEnum.Stop
        };
        panel.AddThemeStyleboxOverride("panel", CreateRowStyle());

        var root = new VBoxContainer();
        root.AddThemeConstantOverride("separation", 4);
        panel.AddChild(root);

        var top = new HBoxContainer();
        top.AddThemeConstantOverride("separation", 8);
        root.AddChild(top);

        top.AddChild(new ColorRect
        {
            Color = gameEvent.IsRead ? Color.FromHtml("#a08060") : Color.FromHtml("#c9a84c"),
            CustomMinimumSize = new Vector2(10f, 10f)
        });

        top.AddChild(new Label { Text = $"Turn {gameEvent.TurnNumber}" });
        top.AddChild(new Label { Text = gameEvent.GameDate.ToString("yyyy-MM-dd") });
        top.AddChild(new Label
        {
            Text = gameEvent.Category.ToString().ToUpperInvariant(),
            Modulate = Color.FromHtml("#c9a84c")
        });
        top.AddChild(new Label
        {
            Text = gameEvent.Importance.ToString().ToUpperInvariant(),
            Modulate = Color.FromHtml("#a08060"),
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        });

        root.AddChild(new Label
        {
            Text = gameEvent.Title,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        });

        panel.GuiInput += @event =>
        {
            if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
            {
                EventSelected?.Invoke(gameEvent);
            }
        };

        return panel;
    }

    private static Button CreateFilterButton(string text, Action action)
    {
        var button = new Button
        {
            Text = text,
            Alignment = HorizontalAlignment.Left
        };
        button.Pressed += action;
        return button;
    }

    private static StyleBoxFlat CreatePanelStyle() => new()
    {
        BgColor = Color.FromHtml("#1a1208"),
        BorderColor = Color.FromHtml("#7a5d2c"),
        BorderWidthTop = 1,
        BorderWidthRight = 1,
        BorderWidthBottom = 1,
        BorderWidthLeft = 1
    };

    private static StyleBoxFlat CreateRowStyle() => new()
    {
        BgColor = Color.FromHtml("#21170b"),
        BorderColor = Color.FromHtml("#5a4523"),
        BorderWidthTop = 1,
        BorderWidthRight = 1,
        BorderWidthBottom = 1,
        BorderWidthLeft = 1,
        ContentMarginTop = 8,
        ContentMarginRight = 10,
        ContentMarginBottom = 8,
        ContentMarginLeft = 10
    };
}
