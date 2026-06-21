using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Godot;
using EmpiresOfHistoryV2.Core;
using EmpiresOfHistoryV2.Events;
using EmpiresOfHistoryV2.Map.Models;
using EmpiresOfHistoryV2.Map.Rendering;
using EmpiresOfHistoryV2.UI.Events;
using EmpiresOfHistoryV2.UI.Theme;

namespace EmpiresOfHistoryV2.UI;

public partial class WorldMapScreen : Control
{
    private const int MajorOrCriticalThreshold = 51;
    private TopBar _topBar = null!;
    private NationInfoPanel _nationInfoPanel = null!;
    private WorldMapManager _worldMapManager = null!;
    private TurnControls? _turnControls;
    private SaveLoadDialog? _saveLoadDialog;
    private BottomTabBar? _bottomTabBar;
    private EventFeedPanel? _eventFeedPanel;
    private EventPopupWindow? _eventPopupWindow;
    private EventArchiveScreen? _eventArchiveScreen;
    private readonly SaveSystem _saveSystem = new();

    public override void _Ready()
    {
        Theme = EmpiresDarkTheme.Create();

        _topBar = GetNode<TopBar>("%TopBar");
        _nationInfoPanel = GetNode<NationInfoPanel>("%NationInfoPanel");
        _worldMapManager = GetNode<WorldMapManager>("%WorldMapManager");
        _turnControls = GetNodeOrNull<TurnControls>("%TurnControls");
        _saveLoadDialog = GetNodeOrNull<SaveLoadDialog>("%SaveLoadDialog");
        _bottomTabBar = GetNodeOrNull<BottomTabBar>("%BottomTabBar");
        _eventFeedPanel = GetNodeOrNull<EventFeedPanel>("%EventFeedPanel");
        _eventPopupWindow = GetNodeOrNull<EventPopupWindow>("%EventPopupWindow");
        _eventArchiveScreen = GetNodeOrNull<EventArchiveScreen>("%EventArchiveScreen");

        _worldMapManager.NationSelected += OnNationSelected;
        GameManager.Instance.EventSystem.TurnEventsResolved += OnTurnEventsResolved;

        if (_saveLoadDialog != null)
        {
            _saveLoadDialog.LoadCompleted += OnSaveLoaded;
        }

        if (_bottomTabBar != null)
        {
            _bottomTabBar.EventsRequested += OnEventsRequested;
        }

        if (_eventFeedPanel != null)
        {
            _eventFeedPanel.EventSelected += OnEventSelected;
        }

        if (_eventPopupWindow != null)
        {
            _eventPopupWindow.ArchiveRequested += OnArchiveRequested;
        }

        if (_eventArchiveScreen != null)
        {
            _eventArchiveScreen.EventSelected += OnArchiveEventSelected;
        }

        var state = GameManager.Instance.GameState;
        _topBar.SetDateTurn(state.CurrentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), state.CurrentTurn);

        if (_turnControls != null)
        {
            _topBar.AttachRightControl(_turnControls);
            _turnControls.Configure(TurnSystem.Instance, _saveSystem);
            _turnControls.Refresh();
        }

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

        _eventFeedPanel?.RefreshFeed(GameManager.Instance.EventSystem.History);
        _eventArchiveScreen?.Refresh(GameManager.Instance.EventSystem.History);
    }

    public override void _ExitTree()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EventSystem.TurnEventsResolved -= OnTurnEventsResolved;
        }

        if (_saveLoadDialog != null)
        {
            _saveLoadDialog.LoadCompleted -= OnSaveLoaded;
        }

        if (_bottomTabBar != null)
        {
            _bottomTabBar.EventsRequested -= OnEventsRequested;
        }

        if (_eventFeedPanel != null)
        {
            _eventFeedPanel.EventSelected -= OnEventSelected;
        }

        if (_eventPopupWindow != null)
        {
            _eventPopupWindow.ArchiveRequested -= OnArchiveRequested;
        }

        if (_eventArchiveScreen != null)
        {
            _eventArchiveScreen.EventSelected -= OnArchiveEventSelected;
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
        _turnControls?.Refresh();

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

    private void OnTurnEventsResolved(IReadOnlyList<GameEvent> events)
    {
        _eventFeedPanel?.RefreshFeed(GameManager.Instance.EventSystem.History);
        _eventArchiveScreen?.Refresh(GameManager.Instance.EventSystem.History);

        var majorOrCritical = events.FirstOrDefault(gameEvent => gameEvent.ImportanceScore >= MajorOrCriticalThreshold);
        if (majorOrCritical != null)
        {
            ShowEvent(majorOrCritical);
        }
    }

    private void OnEventsRequested()
    {
        _eventPopupWindow?.Hide();
        _eventArchiveScreen?.ShowArchive(GameManager.Instance.EventSystem.History);
    }

    private void OnArchiveRequested()
    {
        OnEventsRequested();
    }

    private void OnEventSelected(GameEvent gameEvent)
    {
        ShowEvent(gameEvent);
    }

    private void OnArchiveEventSelected(GameEvent gameEvent)
    {
        _eventArchiveScreen?.Hide();
        ShowEvent(gameEvent);
    }

    private void ShowEvent(GameEvent gameEvent)
    {
        _eventPopupWindow?.ShowEvent(gameEvent);
        _eventFeedPanel?.RefreshFeed(GameManager.Instance.EventSystem.History);
        _eventArchiveScreen?.Refresh(GameManager.Instance.EventSystem.History);
    }
}
