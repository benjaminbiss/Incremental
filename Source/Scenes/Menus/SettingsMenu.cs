using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class SettingsMenu : Control
{
    [Signal]
    public delegate void BackButtonPressedEventHandler();

    private SaveManager saveManager;

    [Export, ExportCategory("GUI Sub-Scenes")]
    private PackedScene settingsOptionSliderScene;

    [Export, ExportCategory("GUI Elements")]
    private NodePath backButtonPath;
    private Button backButton;

    [Export]
    private NodePath gameSettingsContainerPath;
    private VBoxContainer gameSettingsContainer;

    [Export]
    private NodePath audioSettingsContainerPath;
    private VBoxContainer audioSettingsContainer;

    [Export]
    private NodePath controlsSettingsContainerPath;
    private VBoxContainer controlsSettingsContainer;

    public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {GetType().Name} | Initialization failed.");
		}

        SetupMenu();
    }

	private bool Initialize()
	{
		bool result = true;
        bool check;

        saveManager = (SaveManager)GetNode("/root/SaveManager");
        result = result == true ? CheckResource(saveManager, "SaveManager") : result;

        backButton = GetNodeOrNull<Button>(backButtonPath);
        check = CheckResource(backButton, "BackButton");
        if (check)
            backButton.Pressed += OnBackButtonPressed;
        result = result == true ? check : result;

        gameSettingsContainer = GetNodeOrNull<VBoxContainer>(gameSettingsContainerPath);
        result = result == true ? CheckResource(gameSettingsContainer, "GameSettingsContainer") : result;

        audioSettingsContainer = GetNodeOrNull<VBoxContainer>(audioSettingsContainerPath);
        result = result == true ? CheckResource(audioSettingsContainer, "AudioSettingsContainer") : result;

        controlsSettingsContainer = GetNodeOrNull<VBoxContainer>(controlsSettingsContainerPath);
        result = result == true ? CheckResource(controlsSettingsContainer, "ControlsSettingsContainer") : result;

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

    private void SetupMenu()
    {
        SetupSettings(saveManager.Settings.GameSettings, gameSettingsContainer);
        SetupSettings(saveManager.Settings.AudioSettings, audioSettingsContainer);
        SetupSettings(saveManager.Settings.ControlsSettings, controlsSettingsContainer);
    }

    private void SetupSettings(Dictionary<string, Dictionary<string, float>> settings, VBoxContainer container)
    {
        foreach (var setting in settings)
        {
            SettingsOption_Slider option = settingsOptionSliderScene.Instantiate<SettingsOption_Slider>();
            container.AddChild(option);
            option.Name = setting.Key;
            SetupSettingsInfo(option, setting.Key, setting.Value, container);
        }
    }

    private void SetupSettingsInfo(SettingsOption_Slider option, string name, Dictionary<string, float> setting, VBoxContainer container)
    {
        float currentValue = 0f;
        float minValue = 0f;
        float maxValue = 0f;
        foreach (var param in setting)
        {
            switch (param.Key)
            {
                case "Value":
                    currentValue = param.Value;
                    break;
                case "Min":
                    minValue = param.Value;
                    break;
                case "Max":
                    maxValue = param.Value;
                    break;
            }
        }
        option.SetupOption(name, currentValue, minValue, maxValue);
    }

    private void ResetMenu()
    {
        ResetSettings(saveManager.Settings.GameSettings, gameSettingsContainer);
        ResetSettings(saveManager.Settings.AudioSettings, audioSettingsContainer);
        ResetSettings(saveManager.Settings.ControlsSettings, controlsSettingsContainer);
    }

    private void ResetSettings(Dictionary<string, Dictionary<string, float>> settings, VBoxContainer container)
    {
        foreach (Node child in container.GetChildren())
        {
            foreach (var setting in settings)
            {
                if (child.Name == setting.Key)
                {
                    SettingsOption_Slider option = (SettingsOption_Slider)child;
                    ResetSettingsInfo(option, setting.Key, setting.Value, container);
                }
            }
        }
    }

    private void ResetSettingsInfo(SettingsOption_Slider option, string name, Dictionary<string, float> setting, VBoxContainer container)
    {
        float currentValue = 0f;
        float minValue = 0f;
        float maxValue = 0f;
        foreach (var param in setting)
        {
            switch (param.Key)
            {
                case "Value":
                    currentValue = param.Value;
                    break;
                case "Min":
                    minValue = param.Value;
                    break;
                case "Max":
                    maxValue = param.Value;
                    break;
            }
        }
        option.SetupOption(name, currentValue, minValue, maxValue);
    }

    private void UpdateSettings()
    {
        UpdateSettingsInfo(saveManager.Settings.GameSettings, gameSettingsContainer);
        UpdateSettingsInfo(saveManager.Settings.AudioSettings, audioSettingsContainer);
        UpdateSettingsInfo(saveManager.Settings.ControlsSettings, controlsSettingsContainer);
    }

    private void UpdateSettingsInfo(Dictionary<string, Dictionary<string, float>> settings, VBoxContainer container)
    {
        foreach (Node child in container.GetChildren())
        {
            foreach (var setting in settings)
            {
                if (child.Name == setting.Key)
                {
                    SettingsOption_Slider option = (SettingsOption_Slider)child;
                    settings[child.Name]["Value"] = option.valueLabel.Text.ToFloat();
                }
            }
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Visible == true && Input.IsActionJustPressed("ui_cancel"))
        {
            ResetMenu();
            EmitSignal(SignalName.BackButtonPressed);
        }
    }

    private void OnBackButtonPressed()
    {
        UpdateSettings();
        saveManager.SaveSettings();
        EmitSignal(SignalName.BackButtonPressed);
    }

    private void OnChangedConfirmed()
    {
        saveManager.SaveSettings();
    }
}

