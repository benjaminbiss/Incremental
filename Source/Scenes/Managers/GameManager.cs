using Godot;

public partial class GameManager : Node
{
    [Export]
    private PackedScene entityManagerScene;
    private EntityManager entityManager;
    
	[Export]
    private PackedScene gameboardScene;
    private Gameboard gameboard;

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

        entityManager = entityManagerScene.Instantiate<EntityManager>();
        AddChild(entityManager);
        check = CheckResource(entityManager, "EntityManager");
        if (check)
        {

        }
        result = result == true ? check : result;

        gameboard = gameboardScene.Instantiate<Gameboard>();
        AddChild(gameboard);
        check = CheckResource(gameboard, "ResourceName");
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

