using Godot;
using Godot.Collections;

public partial class GameManager : Node
{
    private int previewTowerIndex = -1;
    private TowerBase previewTower;

    [Export]
    private PackedScene entityManagerScene;
    public EntityManager entityManager { get; private set; }
    
	[Export]
    private PackedScene gameboardScene;
    public Gameboard gameboard { get; private set; }

    [Export]
    public Array<PackedScene> towerScenes { get; private set; } = [];
    [Export]
    public Array<PackedScene> enemieScenes { get; private set; } = [];

    public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {GetType().Name} | Initialization failed.");
            return;
        }

        entityManager.TowerSelected += gameboard.OnTowerSelected;
        entityManager.TowerPlaced += gameboard.OnTowerPlaced;
        gameboard.GridCellClicked += entityManager.OnGridCellClicked;
    }

	private bool Initialize()
	{
		bool result = true;
		bool check;

        // EntityManager
        entityManager = entityManagerScene.Instantiate<EntityManager>();
        AddChild(entityManager);
        check = CheckResource(entityManager, "EntityManager");
        if (check)
        {

        }
        result = result == true ? check : result;

        // Gameboard
        gameboard = gameboardScene.Instantiate<Gameboard>();
        AddChild(gameboard);
        check = CheckResource(gameboard, "ResourceName");
		if (check)
		{
			gameboard.GridCellClicked += Gameboard_OnCellClicked;
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

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (previewTowerIndex != -1 && previewTower != null)
        {
            previewTower.SetupTower(gameboard.cellUnderMousePosition, false);
            gameboard.ShowTowerPlacementPreview(previewTower.cellsInRange);
            if (Input.IsActionJustPressed("Right_Click"))
            {
                DeletePreviewTower();
                gameboard.ShowTowerPlacementPreview(null);
                previewTowerIndex = -1;
            }
        }    
    }

    private void Gameboard_OnCellClicked(Vector2I cellPosition)
    {
        if (Input.IsActionPressed("Multi"))
        {
            HandleCellMultiClicked(cellPosition);
        }
        else
        {
            HandleCellClicked(cellPosition);
        }
    }

    private void HandleCellClicked(Vector2I cellPosition)
    {
        if (previewTowerIndex != -1 || previewTower != null)
        {
            TowerBase tower = previewTower;
            ClearPreviewTower();
            entityManager.OnPlaceTower(cellPosition, tower);
            gameboard.ShowTowerPlacementPreview(null);
            previewTowerIndex = -1;
        }
        else
        {
            // IMPLEMENT CLICK SELECT LOGIC HERE
            Node2D entity = entityManager.GetEntityAtPosition(cellPosition);
        }
    }

    private void HandleCellMultiClicked(Vector2I cellPosition)
    {
        if (previewTowerIndex != -1 || previewTower != null)
        {
            TowerBase tower = (TowerBase)previewTower.Duplicate();
            entityManager.OnPlaceTower(cellPosition, tower);
        }
        else
        {
            // IMPLEMENT MULTI-CLICK SELECT LOGIC HERE
            Node2D entity = entityManager.GetEntityAtPosition(cellPosition);
            
        }
    }

    public void OnPlaceTowerButtonPressed(int index)
    {
        DeletePreviewTower();

        if (previewTowerIndex == index)
        {
            previewTowerIndex = -1;
            gameboard.ShowTowerPlacementPreview(null);
            return;
        }
            
        previewTowerIndex = index;
        TowerBase tower = towerScenes[index].Instantiate<TowerBase>();
        previewTower = tower;
        AddChild(tower);
        tower.Name = "Preview Tower";
    }

    private void ClearPreviewTower()
    {
        if (previewTower != null)
        {
            RemoveChild(previewTower);
            previewTower = null;
        }
    }

    private void DeletePreviewTower()
    {
        if (previewTower != null)
        {
            previewTower.QueueFree();
            previewTower = null;
        }
    }
}

