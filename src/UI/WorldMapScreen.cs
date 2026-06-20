using System.Globalization;
using Godot;
using EmpiresOfHistoryV2.Core;
using EmpiresOfHistoryV2.Map.Models;
using EmpiresOfHistoryV2.Map.Rendering;
using EmpiresOfHistoryV2.UI.Theme;

namespace EmpiresOfHistoryV2.UI;

public partial class WorldMapScreen : Control
{
    private TopBar _topBar = null!;
    private NationInfoPanel _nationInfoPanel = null!;
    private WorldMapManager _worldMapManager = null!;
    private TurnControls _turnControls = null!;
    private SaveLoadDialog _saveLoadDialog = null!;
    private readonly SaveSystem _saveSystem = new();

    public override void _Ready()
    {
        Theme = EmpiresDarkTheme.Create();

        _topBar = GetNode<TopBar>("%TopBar");
        _nationInfoPanel = GetNode<NationInfoPanel>("%NationInfoPanel");
        _worldMapManager = GetNode<WorldMapManager>("%WorldMapManager");
        _turnControls = GetNode<TurnControls>("%TurnControls");
        _saveLoadDialog = GetNode<SaveLoadDialog>("%SaveLoadDialog");

        _worldMapManager.NationSelected += OnNationSelected;
        _saveLoadDialog.LoadCompleted += OnSaveLoaded;

        var state = GameManager.Instance.GameState;
        _topBar.SetDateTurn(state.CurrentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), state.CurrentTurn);
        _topBar.AttachRightControl(_turnControls);
        _turnControls.Configure(TurnSystem.Instance, _saveSystem);
        _turnControls.Refresh();

        if (state.SelectedNationId is { } nationId)
        {
            var nation = GameManager.Instance.ContentDatabase.GetNation(nationId);
            if (nation != null)
            {
                ShowNation(nation);
            }
        }
        else
        {
            _nationInfoPanel.ShowEmpty();
        }
    }

    private void OnNationSelected(NationData nation)
    {
        ShowNation(nation);
    }

    private void ShowNation(NationData nation)
    {
        _topBar.SetNation(nation.DisplayName);
        _topBar.SetResources(FormatTreasury(nation.Treasury), FormatPopulation(nation.Population));
        _nationInfoPanel.ShowNation(nation);
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

        return value.ToString(CultureInfo.InvariantCulture);
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

        return $"${value:0}";
    }

    private void OnSaveLoaded()
    {
        _turnControls.Refresh();

        var nationId = GameManager.Instance.GameState.SelectedNationId;
        if (nationId == null)
        {
            _nationInfoPanel.ShowEmpty();
            return;
        }

        var nation = GameManager.Instance.ContentDatabase.GetNation(nationId);
        if (nation != null)
        {
            ShowNation(nation);
            _worldMapManager.SelectNation(nationId);
        }
    }
}
