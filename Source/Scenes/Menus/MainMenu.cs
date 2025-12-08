using Godot;

public partial class MainMenu : Control
{
    [Export]
    private NodePath playButtonPath;
    private Button playButton;
    [Export]
    private NodePath saveVBoxPath;
    private VBoxContainer saveVBox;
    [Export]
    private NodePath save1ButtonPath;
    private Button save1Button;
    [Export]
    private NodePath save2ButtonPath;
    private Button save2Button;
    [Export]
    private NodePath save3ButtonPath;
    private Button save3Button;
    [Export]
    private NodePath settingsButtonPath;
    private Button settingsButton;
    [Export]
    private NodePath quitButtonPath;
    private Button quitButton;

    public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {this} | Initialization failed.");
		}

        saveVBox.Visible = false;
    }

	private bool Initialize()
	{
		bool result = true;

        playButton = GetNodeOrNull<Button>(playButtonPath);
        bool check = CheckResource(playButton, "StartButton");
        if (check)
            playButton.Pressed += OnPlayButtonPressed;
        result = result == true ? check : result;

        saveVBox = GetNodeOrNull<VBoxContainer>(saveVBoxPath);
        result = result == true ? CheckResource(saveVBox, "SaveVBox") : result;

        save1Button = GetNodeOrNull<Button>(save1ButtonPath);
        check = CheckResource(save1Button, "Save1Button");
        if (check)
            save1Button.Pressed += () => OnSaveButtonPressed(1);
        result = result == true ? check : result;

        save2Button = GetNodeOrNull<Button>(save2ButtonPath);
        check = CheckResource(save2Button, "Save2Button");
        if (check)
            save2Button.Pressed += () => OnSaveButtonPressed(2);
        result = result == true ? check : result;

        save3Button = GetNodeOrNull<Button>(save3ButtonPath);
        check = CheckResource(save3Button, "Save3Button");
        if (check)
            save3Button.Pressed += () => OnSaveButtonPressed(3);
        result = result == true ? check : result;

        settingsButton = GetNodeOrNull<Button>(settingsButtonPath);
        check = CheckResource(settingsButton, "SettingsButton");
        if (check)
            settingsButton.Pressed += OnSettingsButtonPressed;
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

    private void OnPlayButtonPressed()
    {
        saveVBox.Visible = !saveVBox.Visible;
    }
    private void OnSaveButtonPressed(int id)
    {
        // id would be the index inside a array of save files
    }
    private void OnSettingsButtonPressed()
    {

    }
    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}

