using Godot;
using System.Collections.Generic;
using EmpiresOfHistory.Map;

namespace EmpiresOfHistory.UI
{
    /// <summary>
    /// Root controller for the World Map screen.
    /// Connects WorldMapManager events to the UI panels and handles bottom tab bar clicks.
    ///
    /// Node path assumptions (relative to this script's root node WorldMapScreen):
    ///   MainLayout/TopBar/TopBarLayout/NationNameLabel
    ///   MainLayout/TopBar/TopBarLayout/NationFlag
    ///   MainLayout/CenterArea/MapContainer/SubViewport/MapRoot        ← WorldMapManager
    ///   MainLayout/CenterArea/NationInfoPanel/PanelVBox/NationNameLabel
    ///   MainLayout/CenterArea/NationInfoPanel/PanelVBox/NationTierLabel
    ///   MainLayout/CenterArea/NationInfoPanel/PanelVBox/StatsGrid/GovernmentValueLabel
    ///   MainLayout/CenterArea/NationInfoPanel/PanelVBox/StatsGrid/PopulationValueLabel
    ///   MainLayout/CenterArea/NationInfoPanel/PanelVBox/StatsGrid/TreasuryValueLabel
    ///   MainLayout/CenterArea/NationInfoPanel/PanelVBox/StatsGrid/AccuracyValueLabel
    ///   MainLayout/CenterArea/NationInfoPanel/PanelVBox/Official1Label
    ///   MainLayout/CenterArea/NationInfoPanel/PanelVBox/Official2Label
    ///   MainLayout/CenterArea/NationInfoPanel/PanelVBox/Official3Label
    ///   MainLayout/CenterArea/NationInfoPanel/PanelVBox/EmptyStateLabel
    ///   MainLayout/BottomTabBar/TabButtons/*TabBtn
    /// </summary>
    [GlobalClass]
    public partial class WorldMapScreenController : Control
    {
        // ── Scene node references ──────────────────────────────────────────────────────

        private WorldMapManager _mapManager;

        // TopBar
        private Label _topBarNationNameLabel;
        private TextureRect _topBarNationFlag;

        // NationInfoPanel
        private Label _nationNameLabel;
        private Label _nationTierLabel;
        private Label _governmentValueLabel;
        private Label _populationValueLabel;
        private Label _treasuryValueLabel;
        private Label _accuracyValueLabel;
        private Label _official1Label;
        private Label _official2Label;
        private Label _official3Label;
        private Label _emptyStateLabel;

        // ── Lifecycle ──────────────────────────────────────────────────────────────────

        public override void _Ready()
        {
            CacheNodeReferences();
            ConnectMapManagerSignals();
            ConnectTabBarButtons();
            ApplyTheme();
            ShowEmptyState(true);
        }

        // ── Node caching ───────────────────────────────────────────────────────────────

        private void CacheNodeReferences()
        {
            _mapManager = GetNodeOrNull<WorldMapManager>(
                "MainLayout/CenterArea/MapContainer/SubViewport/MapRoot");

            if (_mapManager == null)
                GD.PushWarning("WorldMapScreenController: WorldMapManager not found at expected path.");

            // TopBar
            _topBarNationNameLabel = GetNodeOrNull<Label>("MainLayout/TopBar/TopBarLayout/NationNameLabel");
            _topBarNationFlag      = GetNodeOrNull<TextureRect>("MainLayout/TopBar/TopBarLayout/NationFlag");

            // NationInfoPanel
            const string panelRoot = "MainLayout/CenterArea/NationInfoPanel/PanelVBox";
            _nationNameLabel      = GetNodeOrNull<Label>($"{panelRoot}/NationNameLabel");
            _nationTierLabel      = GetNodeOrNull<Label>($"{panelRoot}/NationTierLabel");

            const string statsRoot = $"{panelRoot}/StatsGrid";
            _governmentValueLabel = GetNodeOrNull<Label>($"{statsRoot}/GovernmentValueLabel");
            _populationValueLabel = GetNodeOrNull<Label>($"{statsRoot}/PopulationValueLabel");
            _treasuryValueLabel   = GetNodeOrNull<Label>($"{statsRoot}/TreasuryValueLabel");
            _accuracyValueLabel   = GetNodeOrNull<Label>($"{statsRoot}/AccuracyValueLabel");

            _official1Label  = GetNodeOrNull<Label>($"{panelRoot}/Official1Label");
            _official2Label  = GetNodeOrNull<Label>($"{panelRoot}/Official2Label");
            _official3Label  = GetNodeOrNull<Label>($"{panelRoot}/Official3Label");
            _emptyStateLabel = GetNodeOrNull<Label>($"{panelRoot}/EmptyStateLabel");
        }

        // ── Signal connections ─────────────────────────────────────────────────────────

        private void ConnectMapManagerSignals()
        {
            if (_mapManager == null) return;
            _mapManager.NationSelected += OnNationSelected;
            _mapManager.ProvinceSelected += OnProvinceSelected;
        }

        private void ConnectTabBarButtons()
        {
            ConnectTabButton("MainLayout/BottomTabBar/TabButtons/GovernmentTabBtn",   "GOVERNMENT");
            ConnectTabButton("MainLayout/BottomTabBar/TabButtons/EconomyTabBtn",      "ECONOMY");
            ConnectTabButton("MainLayout/BottomTabBar/TabButtons/MilitaryTabBtn",     "MILITARY");
            ConnectTabButton("MainLayout/BottomTabBar/TabButtons/DiplomacyTabBtn",    "DIPLOMACY");
            ConnectTabButton("MainLayout/BottomTabBar/TabButtons/TechnologyTabBtn",   "TECHNOLOGY");
            // Religion tab is a placeholder — non-functional, grayed out in theme.
            ConnectTabButton("MainLayout/BottomTabBar/TabButtons/ReligionTabBtn",     "RELIGION");
            ConnectTabButton("MainLayout/BottomTabBar/TabButtons/IntelligenceTabBtn", "INTELLIGENCE");
            ConnectTabButton("MainLayout/BottomTabBar/TabButtons/GIATabBtn",          "GIA");
            ConnectTabButton("MainLayout/BottomTabBar/TabButtons/MapModesTabBtn",     "MAP MODES");
        }

        private void ConnectTabButton(string nodePath, string tabName)
        {
            var btn = GetNodeOrNull<Button>(nodePath);
            if (btn == null) return;
            btn.Pressed += () => OnTabPressed(tabName);
        }

        // ── Event handlers ─────────────────────────────────────────────────────────────

        private void OnNationSelected(string nationId)
        {
            if (string.IsNullOrEmpty(nationId))
            {
                ShowEmptyState(true);
                UpdateTopBarNation(null);
                return;
            }

            var nation = _mapManager?.GetNation(nationId);
            if (nation == null)
            {
                ShowEmptyState(true);
                return;
            }

            ShowEmptyState(false);
            PopulateNationPanel(nation);
            UpdateTopBarNation(nation);
        }

        private void OnProvinceSelected(string provinceId)
        {
            // Province selection is handled by deriving the nation from its owner.
            // Additional province-specific UI can be added here in a future phase.
        }

        private void OnTabPressed(string tabName)
        {
            // Tab switching is not implemented in Phase 2.
            // This placeholder logs the click for debugging and future wiring.
            GD.Print($"[WorldMapScreen] Tab pressed: {tabName}");
        }

        // ── Panel population ───────────────────────────────────────────────────────────

        private void PopulateNationPanel(Nation nation)
        {
            SetLabelText(_nationNameLabel, nation.Name);
            SetLabelText(_nationTierLabel, FormatTier(nation.Tier));
            SetLabelText(_governmentValueLabel, FormatGovernment(nation.Government));
            SetLabelText(_populationValueLabel, FormatPopulation(nation.Population));
            SetLabelText(_treasuryValueLabel, FormatTreasury(nation.Treasury));
            // PLACEHOLDER: AccuracyValue is a seed constant — real calculation pending.
            SetLabelText(_accuracyValueLabel, $"{nation.HistoricalAccuracyScore:F1}%");

            // PLACEHOLDER: TopOfficials list from seed data — real roster pending.
            SetOfficialLabel(_official1Label, nation.TopOfficials, 0);
            SetOfficialLabel(_official2Label, nation.TopOfficials, 1);
            SetOfficialLabel(_official3Label, nation.TopOfficials, 2);
        }

        private void UpdateTopBarNation(Nation nation)
        {
            if (_topBarNationNameLabel != null)
                _topBarNationNameLabel.Text = nation?.ShortName ?? string.Empty;
        }

        private void ShowEmptyState(bool show)
        {
            SetNodeVisible(_emptyStateLabel,      show);
            SetNodeVisible(_nationNameLabel,      !show);
            SetNodeVisible(_nationTierLabel,      !show);
            SetNodeVisible(_governmentValueLabel, !show);
            SetNodeVisible(_populationValueLabel, !show);
            SetNodeVisible(_treasuryValueLabel,   !show);
            SetNodeVisible(_accuracyValueLabel,   !show);
            SetNodeVisible(_official1Label,       !show);
            SetNodeVisible(_official2Label,       !show);
            SetNodeVisible(_official3Label,       !show);
        }

        // ── Formatting helpers ─────────────────────────────────────────────────────────

        private static string FormatTier(NationTier tier) => tier switch
        {
            NationTier.GreatPower      => "Great Power",
            NationTier.RegionalPower   => "Regional Power",
            NationTier.DevelopingNation=> "Developing Nation",
            NationTier.MicroState      => "Micro State",
            _                          => tier.ToString()
        };

        private static string FormatGovernment(GovernmentType gov) => gov switch
        {
            GovernmentType.PresidentialRepublic  => "Presidential Republic",
            GovernmentType.ParliamentaryRepublic => "Parliamentary Republic",
            GovernmentType.Monarchy              => "Monarchy",
            GovernmentType.AbsoluteMonarchy      => "Absolute Monarchy",
            GovernmentType.Dictatorship          => "Dictatorship",
            GovernmentType.CommunistState        => "Communist State",
            GovernmentType.Theocracy             => "Theocracy",
            GovernmentType.Tribal                => "Tribal",
            _                                    => gov.ToString()
        };

        private static string FormatPopulation(long pop)
        {
            if (pop >= 1_000_000_000)
                return $"{pop / 1_000_000_000.0:F1}B";
            if (pop >= 1_000_000)
                return $"{pop / 1_000_000.0:F1}M";
            if (pop >= 1_000)
                return $"{pop / 1_000.0:F1}K";
            return pop.ToString();
        }

        private static string FormatTreasury(double treasury)
        {
            if (treasury >= 1_000_000_000_000.0)
                return $"${treasury / 1_000_000_000_000.0:F1}T";
            if (treasury >= 1_000_000_000.0)
                return $"${treasury / 1_000_000_000.0:F1}B";
            if (treasury >= 1_000_000.0)
                return $"${treasury / 1_000_000.0:F1}M";
            return $"${treasury:F0}";
        }

        private static void SetOfficialLabel(Label label, List<Official> officials, int index)
        {
            if (label == null) return;
            if (officials != null && index < officials.Count)
                label.Text = $"{officials[index].Title}: {officials[index].Name}";
            else
                label.Text = string.Empty;
        }

        // ── Tiny utility helpers ───────────────────────────────────────────────────────

        private static void SetLabelText(Label label, string text)
        {
            if (label != null) label.Text = text;
        }

        private static void SetNodeVisible(CanvasItem node, bool visible)
        {
            if (node != null) node.Visible = visible;
        }

        // ── Theme application ──────────────────────────────────────────────────────────

        private void ApplyTheme()
        {
            var theme = EmpiresOfHistory.UI.Theme.EmpiresDarkTheme.Build();
            if (theme != null)
                Theme = theme;
        }
    }
}
