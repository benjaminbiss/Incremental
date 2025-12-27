using Godot;
using System;

public partial class Reward : Control
{
	[Export]
    private NodePath iconPath;
    private TextureRect icon;
	[Export]
    private NodePath namePath;
    private Label name;
    [Export]
    private NodePath quantityPath;
    private Label quantity;

    public Placeables.E_PlatformTypes PlatformType { get; private set; }
    public Placeables.E_TowerTypes TowerType { get; private set; }
    public int Quantity { get; private set; }

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

        icon = GetNodeOrNull<TextureRect>(iconPath);
        check = CheckResource(icon, "Icon");
		result = result == true ? check : result;

        name = GetNodeOrNull<Label>(namePath);
        check = CheckResource(name, "Name");
        result = result == true ? check : result;

        quantity = GetNodeOrNull<Label>(quantityPath);
        check = CheckResource(quantity, "Quantity");
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

	public void SetupPlatformCard(int waveValue, Texture2D texture)
    {
        Placeables.E_PlatformTypes plaformType = GetPlatformFromWave(waveValue);
        PlatformType = plaformType;
        name.Text = plaformType.ToString();
        int amount = waveValue / 3;
        quantity.Text = amount.ToString();
        Quantity = amount;
        SetupIcon(texture, Placeables.platformAtlasRegions[plaformType]);
    }

    public void SetupTowerCard(int waveValue, Texture2D texture)
    {
        Placeables.E_TowerTypes towerType = GetTowerFromWave(waveValue); 
        TowerType = towerType;
        name.Text = towerType.ToString();
        int amount = waveValue / 3;
        quantity.Text = amount.ToString();
        Quantity = amount;
        SetupIcon(texture, Placeables.towerAtlasRegions[towerType]);
    }

    private Placeables.E_TowerTypes GetTowerFromWave(int waveValue)
    {
        Random random = new Random();
        int upper = (int)Placeables.E_TowerTypes.END;
        int towerIndex = random.Next(1, upper);
        return (Placeables.E_TowerTypes)towerIndex;
    }

    private Placeables.E_PlatformTypes GetPlatformFromWave(int waveValue)
    {
        Random random = new Random();
        int upper = (int)Placeables.E_PlatformTypes.END;
        int platformIndex = random.Next(1, upper);
        return (Placeables.E_PlatformTypes)platformIndex;
    }

    private void SetupIcon(Texture2D texture, Rect2 region)
	{
        AtlasTexture atlasTexture = new AtlasTexture();
        atlasTexture.Atlas = texture;
        atlasTexture.Region = region;
        icon.Texture = atlasTexture;
    }
}

