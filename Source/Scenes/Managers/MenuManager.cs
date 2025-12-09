using Godot;
using System;

public partial class MenuManager : Control
{
	[Signal]
	public delegate void StartGameEventHandler(int saveId);
	[Signal]
	public delegate void OnPauseGameEventHandler(bool isPaused);
	[Signal]
	public delegate void QuitGameEventHandler();

    private Control currentMenu;
	private Control previousMenu;

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
			GD.PrintErr($" {GetType().Name} | Initialization failed.");
		}

		mainMenu.Visible = false;
		settingsMenu.Visible = false;
		gameplayMenu.Visible = false;
        pauseMenu.Visible = false;

        ChangeMenu(mainMenu);
    }

    private bool Initialize()
	{
		bool result = true;
		bool check;

        mainMenu = mainMenuScene.Instantiate<MainMenu>();
		AddChild(mainMenu);
        check = CheckResource(mainMenu, "MainMenu");
		if (check)
		{
			mainMenu.SaveButtonPressed += OnStartButtonPressed;
			mainMenu.SettingsButtonPressed += OnSettingsButtonPressed;
			mainMenu.QuitButtonPressed += OnQuitButtonPressed;
        }
        result = result == true ? check : result;	

		settingsMenu = settingsMenuScene.Instantiate<SettingsMenu>();
		AddChild(settingsMenu);
		check = CheckResource(settingsMenu, "SettingsMenu");
		if (check)
		{
			settingsMenu.BackButtonPressed += OnBackButtonPressed;
        }
        result = result == true ? check : result;

        gameplayMenu = gameplayMenuScene.Instantiate<GameplayMenu>();
		AddChild(gameplayMenu);
        result = result == true ? CheckResource(gameplayMenu, "GameplayMenu") : result;

		pauseMenu = pauseMenuScene.Instantiate<PauseMenu>();
		AddChild(pauseMenu);
		check = CheckResource(pauseMenu, "PauseMenu");
		if (check)
		{
			pauseMenu.ResumeButtonPressed += OnResumeButtonPressed;
			pauseMenu.SettingsButtonPressed += OnSettingsButtonPressed;
			pauseMenu.MainMenuButtonPressed += OnMainMenuButtonPressed;
			pauseMenu.QuitButtonPressed += OnQuitButtonPressed;
        }
        result = result == true ? check : result;

        return result;
	}

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (Input.IsActionJustPressed("ui_cancel"))
		{
			GD.Print("MenuManager | ui_cancel pressed.");
			if (currentMenu == gameplayMenu)
			{
				PauseGame();
			}
			else if (currentMenu == pauseMenu)
			{
                ResumeGame();
			}
        }
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

	private void ChangeMenu(Control menuToShow)
	{
		if (currentMenu != null)
			currentMenu.Visible = false;

		previousMenu = currentMenu;
        currentMenu = menuToShow;
		currentMenu.Visible = true;
    }

	private void ResumeGame()
	{
        EmitSignal(SignalName.OnPauseGame, false);
        ChangeMenu(gameplayMenu);
    }

	private void PauseGame()
	{
        EmitSignal(SignalName.OnPauseGame, true);
        ChangeMenu(pauseMenu);
    }

    private void OnStartButtonPressed(int index)
	{
        EmitSignal(SignalName.StartGame, index);		
        ChangeMenu(gameplayMenu);
    }

    private void OnSettingsButtonPressed()
	{
		ChangeMenu(settingsMenu);
    }

    private void OnMainMenuButtonPressed()
    {
        ChangeMenu(mainMenu);
    }
	private void OnQuitButtonPressed()
	{
		EmitSignal(SignalName.QuitGame);
    }

    private void OnBackButtonPressed()
	{
		ChangeMenu(previousMenu);
    }

	private void OnResumeButtonPressed()
	{
		ResumeGame();
    }
}

