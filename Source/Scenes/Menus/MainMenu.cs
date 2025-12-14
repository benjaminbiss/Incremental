using Godot;
using System;

public partial class MainMenu : Control
{
    [Signal]
    public delegate void SaveButtonPressedEventHandler(int saveId);
    [Signal]
    public delegate void SettingsButtonPressedEventHandler();
    [Signal]
    public delegate void QuitButtonPressedEventHandler();

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
            GD.PrintErr($" {GetType().Name} | Initialization failed.");
        }

        saveVBox.Visible = false;
        SaveGlobal saveManager = (SaveGlobal)GetNode("/root/SaveManager");
        float playTime = 0;
        TimeSpan ts = TimeSpan.FromSeconds(playTime);
        string formatted = ts.ToString(@"hh\:mm\:ss");

        if (saveManager.Saves[0] != null)
        {
            playTime = saveManager.Saves[0].PlayTime;
            ts = TimeSpan.FromSeconds(playTime);
            save1Button.Text = $"{saveManager.Saves[0].SaveName} : {ts.ToString(@"hh\:mm\:ss")}";
        }
        if (saveManager.Saves[1] != null)
        {
            playTime = saveManager.Saves[1].PlayTime;
            ts = TimeSpan.FromSeconds(playTime);
            save2Button.Text = $"{saveManager.Saves[1].SaveName} : {ts.ToString(@"hh\:mm\:ss")}";
        }
        if (saveManager.Saves[2] != null)
        {
            playTime = saveManager.Saves[2].PlayTime;
            ts = TimeSpan.FromSeconds(playTime);
            save3Button.Text = $"{saveManager.Saves[2].SaveName} : {ts.ToString(@"hh\:mm\:ss")}";
        }
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
            save1Button.Pressed += () => OnLaunchSaveButtonPressed(0);
        result = result == true ? check : result;

        save2Button = GetNodeOrNull<Button>(save2ButtonPath);
        check = CheckResource(save2Button, "Save2Button");
        if (check)
            save2Button.Pressed += () => OnLaunchSaveButtonPressed(1);
        result = result == true ? check : result;

        save3Button = GetNodeOrNull<Button>(save3ButtonPath);
        check = CheckResource(save3Button, "Save3Button");
        if (check)
            save3Button.Pressed += () => OnLaunchSaveButtonPressed(2);
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
    private void ToggleSaveVBox()
    {
        saveVBox.Visible = !saveVBox.Visible;
    }

    private void OnPlayButtonPressed()
    {
        ToggleSaveVBox();
    }
    private void OnLaunchSaveButtonPressed(int id)
    {
        ToggleSaveVBox();
        EmitSignal(SignalName.SaveButtonPressed, id);
    }
    private void OnSettingsButtonPressed()
    {
        EmitSignal(SignalName.SettingsButtonPressed);
    }
    private void OnQuitButtonPressed()
    {
        EmitSignal(SignalName.QuitButtonPressed);
    }
}

