using Godot;

public partial class EntityManager : Node
{
	[Export]
	private PackedScene computerCoreScene;
    private ComputerCore computerCore;

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

        computerCore = computerCoreScene.Instantiate<ComputerCore>();
        AddChild(computerCore);
        check = CheckResource(computerCore, "ComputerCore");
		if (check)
		{

		}
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
}

