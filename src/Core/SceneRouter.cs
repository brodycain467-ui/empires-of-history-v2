using Godot;

namespace EmpiresOfHistoryV2.Core;

public partial class SceneRouter : Node
{
    public static SceneRouter Instance { get; private set; } = null!;

    public const string MainMenuScene = "res://scenes/MainMenu.tscn";
    public const string NationSelectScene = "res://scenes/NationSelect.tscn";
    public const string WorldMapScene = "res://scenes/Map/WorldMapScreen.tscn";

    public override void _Ready() => Instance = this;

    public void GoTo(string scenePath) => GetTree().ChangeSceneToFile(scenePath);
    public void GoToMainMenu() => GoTo(MainMenuScene);
    public void GoToNationSelect() => GoTo(NationSelectScene);
    public void GoToWorldMap() => GoTo(WorldMapScene);
}
