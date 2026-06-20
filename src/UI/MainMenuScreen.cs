using System.Linq;
using Godot;
using EmpiresOfHistoryV2.Core;
using EmpiresOfHistoryV2.UI.Theme;

namespace EmpiresOfHistoryV2.UI;

public partial class MainMenuScreen : Control
{
    private readonly SaveSystem _saveSystem = new();
    private OptionButton _nationDropdown = null!;
    private VBoxContainer _savesList = null!;
    private Button _continueBtn = null!;
    private SaveLoadDialog _saveLoadDialog = null!;

    public override void _Ready()
    {
        Theme = EmpiresDarkTheme.Create();

        _nationDropdown = GetNode<OptionButton>("%NationDropdown");
        _savesList = GetNode<VBoxContainer>("%SavesList");
        _continueBtn = GetNode<Button>("%ContinueBtn");
        _saveLoadDialog = GetNode<SaveLoadDialog>("%SaveLoadDialog");

        GetNode<Button>("%NewGameBtn").Pressed += () => SceneRouter.Instance.GoToNationSelect();
        GetNode<Button>("%StartGameBtn").Pressed += StartQuickGame;
        GetNode<Button>("%LoadGameBtn").Pressed += () => _saveLoadDialog.ShowDialog(SaveLoadDialog.DialogMode.Load);
        GetNode<Button>("%ExitBtn").Pressed += () => GetTree().Quit();
        _continueBtn.Pressed += ContinueGame;

        _saveLoadDialog.LoadCompleted += OnLoadCompleted;
        _saveLoadDialog.SaveDataChanged += RefreshSaves;

        PopulateNationDropdown();
        RefreshSaves();
    }

    private void PopulateNationDropdown()
    {
        _nationDropdown.Clear();
        var nations = GameManager.Instance.ContentDatabase.GetAllNations().OrderBy(n => n.DisplayName).ToList();
        for (var i = 0; i < nations.Count; i++)
        {
            _nationDropdown.AddItem(nations[i].DisplayName, i);
            _nationDropdown.SetItemMetadata(i, nations[i].Id);
        }

        if (_nationDropdown.ItemCount > 0)
        {
            _nationDropdown.Select(0);
        }
    }

    private void RefreshSaves()
    {
        foreach (var child in _savesList.GetChildren())
        {
            child.QueueFree();
        }

        var slots = _saveSystem.GetAllSaveSlots();
        _continueBtn.Disabled = !slots.Any(s => s != null);

        var hasAny = false;
        for (var i = 0; i < slots.Count; i++)
        {
            var slot = i + 1;
            var data = slots[i];
            if (data == null)
            {
                continue;
            }

            hasAny = true;
            _savesList.AddChild(CreateSaveRow(slot, data));
        }

        if (!hasAny)
        {
            _savesList.AddChild(new Label { Text = "No saves yet" });
        }
    }

    private Control CreateSaveRow(int slot, SaveData data)
    {
        var row = new HBoxContainer();
        var nation = GameManager.Instance.ContentDatabase.GetNation(data.SelectedNationId);

        row.AddChild(new ColorRect
        {
            Color = nation != null ? Color.FromHtml(nation.Color) : Color.FromHtml("#7a7a7a"),
            CustomMinimumSize = new Vector2(16, 16)
        });

        row.AddChild(new Label
        {
            Text = $"{data.SaveName} ({data.GameDate})",
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        });

        var loadBtn = new Button { Text = "LOAD" };
        loadBtn.Pressed += () =>
        {
            if (_saveSystem.Load(slot) != null)
            {
                SceneRouter.Instance.GoToWorldMap();
            }
        };
        row.AddChild(loadBtn);

        return row;
    }

    private void StartQuickGame()
    {
        if (_nationDropdown.ItemCount == 0)
        {
            return;
        }

        var nationId = _nationDropdown.GetItemMetadata(_nationDropdown.Selected).AsString();
        GameManager.Instance.NewGame(nationId);
        SceneRouter.Instance.GoToWorldMap();
    }

    private void ContinueGame()
    {
        // Find first populated slot (slots are 1-indexed, list is 0-indexed)
        var slots = _saveSystem.GetAllSaveSlots();
        for (var i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null)
            {
                if (_saveSystem.Load(i + 1) != null)
                {
                    SceneRouter.Instance.GoToWorldMap();
                }
                return;
            }
        }
    }

    private void OnLoadCompleted()
    {
        RefreshSaves();
    }
}
