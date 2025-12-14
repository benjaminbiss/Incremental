using Godot;

public partial class EnemyBase : Node2D
{
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
		//bool check;
		//check = CheckResource(resource, "ResourceName");
		//if (check)
		//    resource.Pressed += OnResource;
		//result = result == true ? check : result;

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

