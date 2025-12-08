using Godot;

public partial class Main : Node
{
    [Export]
    private PackedScene menuManagerScene;
    private MenuManager menuManager;
    
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
        menuManager = menuManagerScene.Instantiate<MenuManager>();
        AddChild(menuManager);
        result = result == true ? CheckResource(menuManager, "MenuManager") : result;

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

