using System;
using System.Globalization;
using Godot;
using EmpiresOfHistoryV2.Map.Models;

namespace EmpiresOfHistoryV2.UI;

public partial class NationInfoPanel : PanelContainer
{
    private VBoxContainer _statsBox = null!;
    private Label _emptyStateLabel = null!;
    private Label _nameLabel = null!;
    private Label _tierLabel = null!;
    private Label _governmentLabel = null!;
    private Label _populationLabel = null!;
    private Label _treasuryLabel = null!;
    private Label _accuracyLabel = null!;
    private Label[] _officialLabels = null!;

    public override void _Ready()
    {
        CustomMinimumSize = new Vector2(280f, 0f);

        var root = new VBoxContainer();
        AddChild(root);

        _nameLabel = new Label { Text = "Nation", ThemeTypeVariation = "HeaderSmall" };
        _tierLabel = new Label { Text = "Tier" };

        root.AddChild(_nameLabel);
        root.AddChild(_tierLabel);

        _statsBox = new VBoxContainer();
        root.AddChild(_statsBox);

        _governmentLabel = new Label();
        _populationLabel = new Label();
        _treasuryLabel = new Label();
        _accuracyLabel = new Label();

        _statsBox.AddChild(_governmentLabel);
        _statsBox.AddChild(_populationLabel);
        _statsBox.AddChild(_treasuryLabel);
        _statsBox.AddChild(_accuracyLabel);
        _statsBox.AddChild(new HSeparator());
        _statsBox.AddChild(new Label { Text = "TOP OFFICIALS" });

        _officialLabels = [new Label(), new Label(), new Label()];
        foreach (var official in _officialLabels)
        {
            _statsBox.AddChild(official);
        }

        _emptyStateLabel = new Label { Text = "Select a nation to view details.", Visible = false };
        root.AddChild(_emptyStateLabel);

        ShowEmpty();
    }

    public void ShowNation(NationData nation)
    {
        _emptyStateLabel.Visible = false;
        _statsBox.Visible = true;

        _nameLabel.Text = nation.DisplayName;
        _tierLabel.Text = FormatTier(nation.Tier);

        _governmentLabel.Text = $"Government: {FormatGovernment(nation.GovernmentType)}";
        _populationLabel.Text = $"Population: {FormatPopulation(nation.Population)}";
        _treasuryLabel.Text = $"Treasury: {FormatTreasury(nation.Treasury)}";
        _accuracyLabel.Text = $"Hist. Accuracy: {nation.HistoricalAccuracyScore.ToString("0.0", CultureInfo.InvariantCulture)}%"; // PLACEHOLDER

        for (var i = 0; i < _officialLabels.Length; i++)
        {
            if (nation.TopOfficials != null && i < nation.TopOfficials.Count)
            {
                _officialLabels[i].Text = $"{nation.TopOfficials[i].Title}: {nation.TopOfficials[i].Name}"; // PLACEHOLDER
            }
            else
            {
                _officialLabels[i].Text = "— Not assigned —"; // PLACEHOLDER
            }
        }
    }

    public void ShowEmpty()
    {
        _nameLabel.Text = "Nation Info";
        _tierLabel.Text = string.Empty;
        _statsBox.Visible = false;
        _emptyStateLabel.Visible = true;
    }

    private static string FormatPopulation(long value)
    {
        if (value >= 1_000_000_000)
        {
            return $"{value / 1_000_000_000d:0.##}B";
        }

        if (value >= 1_000_000)
        {
            return $"{value / 1_000_000d:0.#}M";
        }

        return value.ToString("N0", CultureInfo.InvariantCulture);
    }

    private static string FormatTreasury(double value)
    {
        if (value >= 1_000_000_000_000d)
        {
            return $"${value / 1_000_000_000_000d:0.#}T";
        }

        if (value >= 1_000_000_000d)
        {
            return $"${value / 1_000_000_000d:0.#}B";
        }

        if (value >= 1_000_000d)
        {
            return $"${value / 1_000_000d:0.#}M";
        }

        return $"${value:0}";
    }

    private static string FormatGovernment(string value)
    {
        var parts = value.Replace('_', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < parts.Length; i++)
        {
            parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i][1..];
        }

        return string.Join(' ', parts);
    }

    private static string FormatTier(string value)
    {
        return FormatGovernment(value);
    }
}
