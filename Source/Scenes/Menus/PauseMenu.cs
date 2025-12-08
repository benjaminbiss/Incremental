using Godot;

public partial class PauseMenu : Control
{
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

