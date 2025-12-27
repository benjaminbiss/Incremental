using Godot;

[Tool]
public partial class Platform : Sprite2D
{
    public string platformName { get; private set; } = "Platform Name";

    private Placeables.E_PlatformTypes platformType;
    [Export]
    public Placeables.E_PlatformTypes PlatformType
    {
        get { return platformType; }
        set { SetPlatformType(value); }
    }
  
    private void SetPlatformType(Placeables.E_PlatformTypes newType)
    {
        platformType = newType;
        if (Placeables.platformAtlasRegions.ContainsKey(platformType))
        {
            RegionRect = Placeables.platformAtlasRegions[platformType];
            platformName = platformType.ToString();
        }
        else
        {
            GD.PrintErr($" Platform | Atlas region for platform type {platformType} not found.");
        }
    }

    public Texture2D GetSpriteTexture()
    {
        return Texture;
    }

    public Rect2 GetSpriteRegion()
    {
        return RegionRect;
    }
}

