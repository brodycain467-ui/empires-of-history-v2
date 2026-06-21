using Godot;
using System.Linq;
using EmpiresOfHistoryV2.Events;
using EmpiresOfHistoryV2.Events.Definitions;
using EmpiresOfHistoryV2.Map.Systems;
using EmpiresOfHistoryV2.Simulation;
using EmpiresOfHistoryV2.Simulation.Systems;

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
    public SimulationManager SimulationManager { get; private set; } = new();
    public DirtyRegionTracker DirtyTracker { get; private set; } = new();

    public override void _Ready()
    {
        Instance = this;

        ContentDatabase.LoadAll();

        var provinces = ContentDatabase.GetAllProvinces();
        var nations = ContentDatabase.GetAllNations();

        OwnershipSystem.Initialize(provinces.ToList());
        NationColorRegistry.Initialize(nations.ToList());
        BorderHistorySystem.Load();

        var loader = new EventDefinitionLoader();
        var definitions = loader.LoadAll();
        var jsonSource = new JsonEventSource(definitions, EventSystem.ChainTracker);
        EventSystem.RegisterSource(jsonSource);

        // Register simulation systems in canonical order
        SimulationManager.RegisterSystem(new TimelineSystem());
        SimulationManager.RegisterSystem(new PopulationSystem());
        SimulationManager.RegisterSystem(new EconomySystem());
        SimulationManager.RegisterSystem(new TechnologySystem());
        SimulationManager.RegisterSystem(new GovernmentSystem());
        SimulationManager.RegisterSystem(new ElectionsSystem());
        SimulationManager.RegisterSystem(new ReligionSystem());
        SimulationManager.RegisterSystem(new IntelligenceSystem());
        SimulationManager.RegisterSystem(new MilitarySystem());
        SimulationManager.RegisterSystem(new GIAAdvisorSystem());

        // Initialize with first-turn context
        var initContext = BuildSimulationContext();
        SimulationManager.Initialize(initContext);
    }

    public SimulationContext BuildSimulationContext() => new()
    {
        TurnNumber = GameState.CurrentTurn,
        GameDate = GameState.CurrentDate,
        ActiveNationId = GameState.SelectedNationId,
        AllNations = ContentDatabase.GetAllNations(),
        ActiveNationProvinceIds = GameState.SelectedNationId != null
            ? OwnershipSystem.GetProvinces(GameState.SelectedNationId)
            : [],
        EventHistory = EventSystem.History,
        InjectEvent = EventSystem.InjectEvent
    };

    public void NewGame(string nationId)
    {
        GameState = new GameState();
        GameState.SelectedNationId = nationId;
    }
}
