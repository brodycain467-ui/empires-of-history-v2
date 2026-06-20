using Godot;
using System.Linq;
using EmpiresOfHistoryV2.Events;
using EmpiresOfHistoryV2.Map.Systems;

namespace EmpiresOfHistoryV2.Core;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; } = null!;

    public ContentDatabase ContentDatabase { get; private set; } = new();
    public GameState GameState { get; private set; } = new();
    public OwnershipSystem OwnershipSystem { get; private set; } = new();
    public NationColorRegistry NationColorRegistry { get; private set; } = new();
    public BorderHistorySystem BorderHistorySystem { get; private set; } = new();
    public EventSystem EventSystem { get; private set; } = new();

    public override void _Ready()
    {
        Instance = this;

        ContentDatabase.LoadAll();

        var provinces = ContentDatabase.GetAllProvinces();
        var nations = ContentDatabase.GetAllNations();

        OwnershipSystem.Initialize(provinces.ToList());
        NationColorRegistry.Initialize(nations.ToList());
        BorderHistorySystem.Load();
        EventSystem.RegisterSource(new PlaceholderEventSource());
    }

    public void NewGame(string nationId)
    {
        GameState = new GameState();
        GameState.SelectedNationId = nationId;
    }
}
