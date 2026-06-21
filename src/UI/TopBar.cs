using Godot;

namespace EmpiresOfHistoryV2.UI;

public partial class TopBar : PanelContainer
{
    private Label _nationLabel = null!;
    private Label _resourceLabel = null!;
    private Label _turnLabel = null!;

    public override void _Ready()
    {
        CustomMinimumSize = new Vector2(0f, 48f);

        var row = new HBoxContainer();
        AddChild(row);

        _nationLabel = new Label { Text = "🏳 Unknown Nation", SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
        _resourceLabel = new Label { Text = "Treasury: $0  |  Population: 0", SizeFlagsHorizontal = Control.SizeFlags.ExpandFill, HorizontalAlignment = HorizontalAlignment.Center };
        _turnLabel = new Label { Text = "2011-01-01 • Turn 1", HorizontalAlignment = HorizontalAlignment.Right, SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };

        row.AddChild(_nationLabel);
        row.AddChild(_resourceLabel);
        row.AddChild(_turnLabel);
    }

    public void SetNation(string nationName)
    {
        _nationLabel.Text = $"🏳 {nationName}";
    }

    public void SetResources(string treasury, string population)
    {
        _resourceLabel.Text = $"Treasury: {treasury}  |  Population: {population}";
    }

    public void SetDateTurn(string date, int turn)
    {
        _turnLabel.Text = $"{date} • Turn {turn}";
    }
}
