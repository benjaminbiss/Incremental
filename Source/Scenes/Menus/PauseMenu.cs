using Godot;

public partial class PauseMenu : Control
{
    [Signal]
    public delegate void ResumeButtonPressedEventHandler();
    [Signal]
    public delegate void SettingsButtonPressedEventHandler();
    [Signal]
    public delegate void MainMenuButtonPressedEventHandler();
    [Signal]
    public delegate void QuitButtonPressedEventHandler();

    [Export]
    private NodePath resumeButtonPath;
    private Button resumeButton;
    [Export]
    private NodePath settingsButtonPath;
    private Button settingsButton;
    [Export]
    private NodePath mainMenuButtonPath;
    private Button mainMenuButton;
    [Export]
    private NodePath quitButtonPath;
    private Button quitButton;

    public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {GetType().Name} | Initialization failed.");
		}
	}

    private bool Initialize()
	{
		bool result = true;
        bool check;

        resumeButton = GetNodeOrNull<Button>(resumeButtonPath);
        check = CheckResource(resumeButton, "ResumeButton");
        if (check)
            resumeButton.Pressed += OnResumeButtonPressed;
        result = result == true ? check : result;

        settingsButton = GetNodeOrNull<Button>(settingsButtonPath);
        check = CheckResource(settingsButton, "SettingsButton");
        if (check)
            settingsButton.Pressed += OnSettingsButtonPressed;
        result = result == true ? check : result;

        mainMenuButton = GetNodeOrNull<Button>(mainMenuButtonPath);
        check = CheckResource(mainMenuButton, "MainMenuButton");
        if (check)
            mainMenuButton.Pressed += OnMainMenuButtonPressed;
        result = result == true ? check : result;

        quitButton = GetNodeOrNull<Button>(quitButtonPath);
        check = CheckResource(quitButton, "QuitButton");
        if (check)
            quitButton.Pressed += OnQuitButtonPressed;
        result = result == true ? check : result;

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

    private void OnResumeButtonPressed()
    {
        EmitSignal(SignalName.ResumeButtonPressed);
    }

    private void OnSettingsButtonPressed()
    {
        EmitSignal(SignalName.SettingsButtonPressed);
    }

    private void OnMainMenuButtonPressed()
    {
        EmitSignal(SignalName.MainMenuButtonPressed);
    }

    private void OnQuitButtonPressed()
    {
        EmitSignal(SignalName.QuitButtonPressed);
        GetTree().Quit();
    }
}

