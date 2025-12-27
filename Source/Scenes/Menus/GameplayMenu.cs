using Godot;
using Godot.Collections;
using System;
using static Placeables;

public partial class GameplayMenu : Control
{
    [Signal]
    public delegate void TowerButtonPressedEventHandler(int index);
    [Signal]
    public delegate void PlatformButtonPressedEventHandler(int index);
    [Signal]
    public delegate void StartWaveButtonPressedEventHandler();

    [Export]
    private PackedScene rewardCardScene;

    [Export]
    private NodePath startWaveButtonPath;
    private Button startWaveButton;
    [Export]
    private NodePath towerButtonsContainerPath;
    private VBoxContainer towerButtonsContainer;
    [Export]
    private NodePath rewardButtonsContainerPath;
    private HBoxContainer rewardButtonsContainer;
    [Export]
    private NodePath toggleRewardsButtonPath;
    private Button toggleRewardsButton;
    [Export]
    private NodePath platformButtonsContainerPath;
    private VBoxContainer platformButtonsContainer;
    [Export]
    private NodePath pointsLabelPath;
    private Label pointsLabel;
    [Export]
    private NodePath waveCounterLabelPath;
    private Label waveCounterLabel;
    [Export]
    private NodePath lifeLabelPath;
    private Label lifeLabel;

    private PackedScene towerScene;
    private PackedScene platformScene;

    public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {GetType().Name} | Initialization failed.");
		}

        ShowRewards(1);
        startWaveButton.Hide();
    }

	private bool Initialize()
	{
        bool result = true;
        bool check;

        // Start Wave Button
        startWaveButton = GetNodeOrNull<Button>(startWaveButtonPath);
        check = CheckResource(startWaveButton, "StartWaveButton");
        if (check)
        {
            startWaveButton.Pressed += OnStartWaveButtonPressed;
        }
        result = result == true ? check : result;

        // Tower Buttons Container
        towerButtonsContainer = GetNodeOrNull<VBoxContainer>(towerButtonsContainerPath);
        result = result == true ? CheckResource(towerButtonsContainer, "TowerButtonsContainer") : result;

        // Reward Buttons Container
        rewardButtonsContainer = GetNodeOrNull<HBoxContainer>(rewardButtonsContainerPath);
        result = result == true ? CheckResource(rewardButtonsContainer, "RewardButtonsContainer") : result;

        // Toggle Rewards Button
        toggleRewardsButton = GetNodeOrNull<Button>(toggleRewardsButtonPath);
        check = CheckResource(toggleRewardsButton, "ToggleRewardsButton");
        if (check)
            toggleRewardsButton.Pressed += OnToggleRewardButtonClicked;
        result = result == true ? check : result;

        // Platform Buttons Container
        platformButtonsContainer = GetNodeOrNull<VBoxContainer>(platformButtonsContainerPath);
        result = result == true ? CheckResource(platformButtonsContainer, "PlatformButtonsContainer") : result;

        // Points Label
        pointsLabel = GetNodeOrNull<Label>(pointsLabelPath);
        result = result == true ? CheckResource(pointsLabel, "PointsLabel") : result;

        // Wave Counter Label
        waveCounterLabel = GetNodeOrNull<Label>(waveCounterLabelPath);
        result = result == true ? CheckResource(waveCounterLabel, "WaveCounterLabel") : result;

        // Life Label
        lifeLabel = GetNodeOrNull<Label>(lifeLabelPath);
        result = result == true ? CheckResource(lifeLabel, "LifeLabel") : result;


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

    private void AddTowerButton(int towerType)
    {
        Button towerButton = new Button();
        towerButtonsContainer.AddChild(towerButton);

        Tower tower = towerScene.Instantiate<Tower>();
        tower.TowerType = (E_TowerTypes)towerType;
        towerButton.Text = tower.towerName;
        towerButton.Name = tower.towerName + "_Button";
        AtlasTexture texture = new AtlasTexture();
        texture.Atlas = tower.GetSpriteTexture();
        texture.Region = tower.GetSpriteRegion();
        towerButton.Icon = texture;

        towerButton.Pressed += () => OnTowerButtonPressed(towerType);
    }

    private void OnTowerButtonPressed(int towerType)
    {
        EmitSignal(SignalName.TowerButtonPressed, towerType);
    }

    private void AddPlatformButton(int platformType)
    {
        Button platformButton = new Button();
        platformButtonsContainer.AddChild(platformButton);

        Platform platform = platformScene.Instantiate<Platform>();
        platform.PlatformType = (E_PlatformTypes)platformType;
        platformButton.Text = platform.platformName;
        platformButton.Name = platform.platformName + "_Button";
        AtlasTexture texture = new AtlasTexture();
        texture.Atlas = platform.GetSpriteTexture();
        texture.Region = platform.GetSpriteRegion();
        platformButton.Icon = texture;

        platformButton.Pressed += () => OnPlatformButtonPressed(platformType);
    }

    private void OnPlatformButtonPressed(int platformType)
    {
        EmitSignal(SignalName.PlatformButtonPressed, platformType);
    }


    private void OnStartWaveButtonPressed()
    {
        EmitSignal(SignalName.StartWaveButtonPressed);
        startWaveButton.Hide();
    }

    public void OnPointsUpdated(int points)
    {
        pointsLabel.Text = points.ToString();
    }

    public void OnWaveIncreased(int waveNumber)
    {
        waveCounterLabel.Text = waveNumber.ToString();
        ShowRewards(waveNumber);
    }

    public void OnLifeUpdated(int life)
    {
        lifeLabel.Text = life.ToString();
    }

    private void ShowRewards(int wave)
    {
        for (int i = 0; i < 3; i++)
        {
            CreateReward(i, wave);
        }

        toggleRewardsButton.Show();
        rewardButtonsContainer.Show();
    }
    private void CreateReward(int i, int wave)
    {
        RewardCard rewardCard = rewardCardScene.Instantiate<RewardCard>();
        rewardButtonsContainer.AddChild(rewardCard);
        rewardCard.Name = "RewardCard_" + i;
        rewardCard.CreateCard(wave);
        rewardCard.AddRewardPlatform += OnPlatformReward;
        rewardCard.AddRewardTower += OnTowerReward;
    }
    private void OnPlatformReward(int platformType, int quantity)
    {
        // ADD PLATFORMS HERE


        HandleReward();
    }
    private void OnTowerReward(int towerType, int quantity)
    {
        // ADD TOWERS HERE


        HandleReward();
    }
    private void HandleReward()
    {
        foreach (Node child in rewardButtonsContainer.GetChildren())
        {
            child.QueueFree();
        }
        toggleRewardsButton.Hide();
        startWaveButton.Show();
    }
    private void OnToggleRewardButtonClicked()
    {
        ToggleRewards();
    }
    private void ToggleRewards()
    {
        if (rewardButtonsContainer.Visible)
            rewardButtonsContainer.Hide();
        else
            rewardButtonsContainer.Show();
    }
}

