using Godot;

namespace EmpiresOfHistoryV2.UI;

public partial class BottomTabBar : PanelContainer
{
    private static readonly string[] Tabs =
    [
        "GOVERNMENT",
        "ECONOMY",
        "MILITARY",
        "DIPLOMACY",
        "TECHNOLOGY",
        "RELIGION",
        "INTELLIGENCE",
        "GIA",
        "MAP MODES"
    ];

    public override void _Ready()
    {
        CustomMinimumSize = new Vector2(0f, 60f);

        var row = new HBoxContainer();
        AddChild(row);

        foreach (var tab in Tabs)
        {
            var button = new Button
            {
                Text = tab,
                Disabled = true,
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
            };

            if (tab == "RELIGION")
            {
                button.Modulate = new Color(1f, 1f, 1f, 0.4f);
            }

            row.AddChild(button);
        }
    }
}
