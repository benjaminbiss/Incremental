using Godot;

public partial class MenuManager : Control
{
	private CanvasLayer canvasLayer;

	[Export]
	private PackedScene mainMenuScene;
	private MainMenu mainMenu;

    [Export]
	private PackedScene settingsMenuScene;
	private SettingsMenu settingsMenu;

    [Export]
	private PackedScene gameplayMenuScene;
	private GameplayMenu gameplayMenu;

	[Export]
	private PackedScene pauseMenuScene;
	private PauseMenu pauseMenu;

    public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {this} | Initialization failed.");
		}
	}

	private bool Initialize()
	{
		bool result = true;

		canvasLayer = GetNodeOrNull<CanvasLayer>("CanvasLayer");
        result = result == true ? CheckResource(canvasLayer, "CanvasLayer") : result;

        mainMenu = mainMenuScene.Instantiate<MainMenu>();
		canvasLayer.AddChild(mainMenu);
        result = result == true ? CheckResource(mainMenu, "MainMenu") : result;	

		settingsMenu = settingsMenuScene.Instantiate<SettingsMenu>();
		canvasLayer.AddChild(settingsMenu);
        result = result == true ? CheckResource(settingsMenu, "SettingsMenu") : result;

        gameplayMenu = gameplayMenuScene.Instantiate<GameplayMenu>();
		canvasLayer.AddChild(gameplayMenu);
        result = result == true ? CheckResource(gameplayMenu, "GameplayMenu") : result;

		pauseMenu = pauseMenuScene.Instantiate<PauseMenu>();
		canvasLayer.AddChild(pauseMenu);
		result = result == true ? CheckResource(pauseMenu, "PauseMenu") : result;

        return result;
	}

	private bool CheckResource(Node resource, string resourceName)
	{
		if (resource == null)
		{
			GD.PrintErr($" {GetType().Name} | {resourceName} is null.");
            return false;
		}
		return true;
    }
}

