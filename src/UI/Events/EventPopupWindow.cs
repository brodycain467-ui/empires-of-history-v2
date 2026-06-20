using System;
using System.Linq;
using Godot;
using EmpiresOfHistoryV2.Events;
using EmpiresOfHistoryV2.UI.Theme;

namespace EmpiresOfHistoryV2.UI.Events;

public partial class EventPopupWindow : Control
{
    private const string PlaceholderEffectKey = "placeholder";
    private const string PlaceholderEffectValue = "true";

    public event Action? ArchiveRequested;

    private Label _categoryLabel = null!;
    private PanelContainer _importanceChip = null!;
    private Label _importanceChipLabel = null!;
    private Label _titleLabel = null!;
    private Label _descriptionLabel = null!;
    private VBoxContainer _effectsRows = null!;
    private Label _effectsHeader = null!;

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

        var center = new CenterContainer
        {
            LayoutMode = 1,
            AnchorRight = 1f,
            AnchorBottom = 1f
        };
        AddChild(center);

        var panel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(600f, 400f)
        };
        panel.AddThemeStyleboxOverride("panel", CreatePanelStyle());
        center.AddChild(panel);

        var margin = new MarginContainer();
        margin.AddThemeConstantOverride("margin_left", 18);
        margin.AddThemeConstantOverride("margin_top", 18);
        margin.AddThemeConstantOverride("margin_right", 18);
        margin.AddThemeConstantOverride("margin_bottom", 18);
        panel.AddChild(margin);

        var root = new VBoxContainer();
        root.AddThemeConstantOverride("separation", 12);
        margin.AddChild(root);

        var header = new HBoxContainer();
        header.AddThemeConstantOverride("separation", 12);
        root.AddChild(header);

        _categoryLabel = new Label
        {
            Text = "GENERAL",
            Modulate = Color.FromHtml("#c9a84c"),
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        header.AddChild(_categoryLabel);

        _importanceChip = new PanelContainer();
        _importanceChip.AddThemeStyleboxOverride("panel", CreateChipStyle(Color.FromHtml("#a08060")));
        _importanceChipLabel = new Label
        {
            Text = "MINOR"
        };
        _importanceChipLabel.AddThemeColorOverride("font_color", Color.FromHtml("#1a1208"));
        _importanceChip.AddChild(_importanceChipLabel);
        header.AddChild(_importanceChip);

        _titleLabel = new Label
        {
            Text = "Event Title",
            Modulate = Color.FromHtml("#c9a84c"),
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
        root.AddChild(_titleLabel);

        var descriptionScroll = new ScrollContainer
        {
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddChild(descriptionScroll);

        _descriptionLabel = new Label
        {
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        descriptionScroll.AddChild(_descriptionLabel);

        _effectsHeader = new Label
        {
            Text = "EFFECTS",
            Modulate = Color.FromHtml("#c9a84c")
        };
        root.AddChild(_effectsHeader);

        _effectsRows = new VBoxContainer();
        _effectsRows.AddThemeConstantOverride("separation", 6);
        root.AddChild(_effectsRows);

        var actions = new HBoxContainer
        {
            Alignment = BoxContainer.AlignmentMode.End
        };
        actions.AddThemeConstantOverride("separation", 8);
        root.AddChild(actions);

        var archiveButton = new Button { Text = "ARCHIVE" };
        archiveButton.Pressed += () => ArchiveRequested?.Invoke();
        actions.AddChild(archiveButton);

        var dismissButton = new Button { Text = "DISMISS" };
        dismissButton.Pressed += Hide;
        actions.AddChild(dismissButton);
    }

    public void ShowEvent(GameEvent gameEvent)
    {
        gameEvent.IsRead = true;
        _categoryLabel.Text = gameEvent.Category.ToString().ToUpperInvariant();
        _importanceChipLabel.Text = gameEvent.Importance.ToString().ToUpperInvariant();
        _importanceChip.AddThemeStyleboxOverride("panel", CreateChipStyle(GetImportanceColor(gameEvent.Importance)));
        _titleLabel.Text = gameEvent.Title;
        _descriptionLabel.Text = gameEvent.Description;

        foreach (var child in _effectsRows.GetChildren())
        {
            child.QueueFree();
        }

        var visibleEffects = gameEvent.Effects.Where(pair => !IsPlaceholderEffect(pair)).ToList();
        _effectsHeader.Visible = visibleEffects.Count > 0;
        _effectsRows.Visible = visibleEffects.Count > 0;

        foreach (var effect in visibleEffects)
        {
            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", 8);

            row.AddChild(new Label
            {
                Text = effect.Key.ToUpperInvariant(),
                CustomMinimumSize = new Vector2(140f, 0f),
                Modulate = Color.FromHtml("#c9a84c")
            });
            row.AddChild(new Label
            {
                Text = effect.Value,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            });

            _effectsRows.AddChild(row);
        }

        Visible = true;
    }

    private static StyleBoxFlat CreatePanelStyle() => new()
    {
        BgColor = Color.FromHtml("#1a1208"),
        BorderColor = Color.FromHtml("#c9a84c"),
        BorderWidthTop = 1,
        BorderWidthRight = 1,
        BorderWidthBottom = 1,
        BorderWidthLeft = 1
    };

    private static StyleBoxFlat CreateChipStyle(Color color) => new()
    {
        BgColor = color,
        ContentMarginTop = 4,
        ContentMarginRight = 8,
        ContentMarginBottom = 4,
        ContentMarginLeft = 8
    };

    private static Color GetImportanceColor(EventImportance importance) => importance switch
    {
        EventImportance.Minor => Color.FromHtml("#a08060"),
        EventImportance.Moderate => Color.FromHtml("#f0e6cc"),
        EventImportance.Major => Color.FromHtml("#c9a84c"),
        EventImportance.Critical => Color.FromHtml("#e0b347"),
        _ => Color.FromHtml("#a08060")
    };

    private static bool IsPlaceholderEffect(System.Collections.Generic.KeyValuePair<string, string> effect) =>
        effect.Key == PlaceholderEffectKey && effect.Value == PlaceholderEffectValue;
}
