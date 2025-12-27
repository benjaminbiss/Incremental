using Godot;
using System;

public partial class RewardCard : Control
{
	[Signal]
	public delegate void AddRewardPlatformEventHandler(int platformType, int quantity);
	[Signal]
	public delegate void AddRewardTowerEventHandler(int towerType, int quantity);

    [Export]
	private Texture2D towerAtlas;
	[Export]
	private Texture2D platformAtlas;

	[Export]
	private NodePath buttonPath;
	private Button button;
	[Export]
	private NodePath rewardOnePath;
    private Reward rewardOne;
	[Export]
    private NodePath rewardTwoPath;
    private Reward rewardTwo;

    public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {GetType().Name} | Initialization failed.");
			return;
		}
	}

	private bool Initialize()
	{
		bool result = true;
		bool check;

		check = towerAtlas != null;
        result = result == true ? check : result;

		check = platformAtlas != null;
		result = result == true ? check : result;

        button = GetNodeOrNull<Button>(buttonPath);
        check = CheckResource(button, "Button");
		if (check)
			button.Pressed += OnRewardButtonPressed;
        result = result == true ? check : result;

		rewardOne = GetNodeOrNull<Reward>(rewardOnePath);
		check = CheckResource(rewardOne, "RewardOne");
		result = result == true ? check : result;

		rewardTwo = GetNodeOrNull<Reward>(rewardTwoPath);
		check = CheckResource(rewardTwo, "RewardTwo");
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

	public void CreateCard(int wave)
	{
        rewardOne.SetupTowerCard(wave, towerAtlas);
        rewardTwo.SetupPlatformCard(wave, platformAtlas);
    }

	private void OnRewardButtonPressed()
	{
		if (rewardOne.PlatformType == Placeables.E_PlatformTypes.NONE)
			EmitSignal(SignalName.AddRewardTower, (int)rewardOne.TowerType, rewardOne.Quantity);
		else
			EmitSignal(SignalName.AddRewardPlatform, (int)rewardOne.PlatformType, rewardOne.Quantity);

		if (rewardTwo.PlatformType == Placeables.E_PlatformTypes.NONE)
			EmitSignal(SignalName.AddRewardTower, (int)rewardTwo.TowerType, rewardTwo.Quantity);
		else
			EmitSignal(SignalName.AddRewardPlatform, (int)rewardTwo.PlatformType, rewardTwo.Quantity);
    }
}

