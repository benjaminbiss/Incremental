using Godot;
using Godot.Collections;

public partial class EntityManager : Node
{
	[Signal]
	public delegate void TowerSelectedEventHandler(TowerBase tower);
	private TowerBase selectedTower;

	[Signal]
	public delegate void TowerPlacedEventHandler(Vector2I cell);

    private Node towerParent;
    public Array<TowerBase> towers { get; private set; } = [];
	private Node enemyParent;
    public Array<EnemyBase> enemies { get; private set; } = [];
    
	[Export]
	private PackedScene defendObjectiveScene;
	private DefendObjective defendObjective;
	[Export]
	private PackedScene entryPointScene;
	private Node2D entryPoint;

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

        // Defend Objective
        defendObjective = defendObjectiveScene.Instantiate<DefendObjective>();
        AddChild(defendObjective);
		defendObjective.Position = new Vector2(0, 125);
        check = CheckResource(defendObjective, "ComputerCore");
		if (check)
		{

		}
		result = result == true ? check : result;

        // Entry Point
        entryPoint = entryPointScene.Instantiate<Node2D>();
        AddChild(entryPoint);
		entryPoint.Position = new Vector2(0, -1055);
        check = CheckResource(entryPoint, "EntryPoint");
        result = result == true ? check : result;


		towerParent = new Node();
		AddChild(towerParent);
		towerParent.Name = "Towers";

        enemyParent = new Node();
        AddChild(enemyParent);
        enemyParent.Name = "Enemies";

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

    public void OnPlaceTower(Vector2I cellPosition, TowerBase tower)
    {
        towerParent.AddChild(tower);
        towers.Add(tower);
		tower.Name = tower.towerName;
		tower.SetupTower(cellPosition, true);
		EmitSignal(SignalName.TowerPlaced, cellPosition);
    }

	public Node2D GetEntityAtPosition(Vector2I cellPosition)
    {
		return null;
    }
}

