using Godot;
using Godot.Collections;
using System;

public partial class GameManager : Node
{
    [Signal]
    public delegate void WaveIncreasedEventHandler(int wave);
    [Signal]
    public delegate void PointsUpdatedEventHandler(int points);
    [Signal]
    public delegate void LifeUpdatedEventHandler(int life);
    [Signal]
    public delegate void GameOverEventHandler();

    private int previewTowerIndex = -1;
    private Tower previewTower;

    private const int startingWave = 1;
    private int waveNumber;
    private const int startingPoints = 10;
    private int points;
    private const int startingLife = 100;
    private int life;

    [Export]
    private PackedScene entityManagerScene;
    public EntityManager entityManager { get; private set; }
    
	[Export]
    private PackedScene gameboardScene;
    public Gameboard gameboard { get; private set; }

    [Export]
    public Array<PackedScene> towerScenes { get; private set; } = [];
    [Export]
    public Array<PackedScene> enemyScenes { get; private set; } = [];

    public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {GetType().Name} | Initialization failed.");
            return;
        }

        entityManager.TowerSelected += gameboard.OnTowerSelected;
        entityManager.TowerPlaced += gameboard.OnTowerPlaced;
        entityManager.TowerPlaced += OnTowerPlaced;
        entityManager.RelayEnemyRequestNewPath += gameboard.OnEnemyRequestNewPath;
        entityManager.PointsAwarded += OnPointsAwarded;
        entityManager.WaveEnded += OnWaveEnded;
        entityManager.DealDamagerToPlayer += OnDealDamageToPlayer;

        ResetGame();

        gameboard.GridCellClicked += entityManager.OnGridCellClicked;
        gameboard.PathUpdated += entityManager.OnWorldPathUpdated;

        gameboard.InitializeGamePath();
    }

    private void OnDealDamageToPlayer(int damage)
    {
        life -= damage;
        life = Mathf.Max(life, 0);
        EmitSignal(SignalName.LifeUpdated, life);
        if (life <= 0)
        {
            EmitSignal(SignalName.GameOver);
        }
    }

    private void OnTowerPlaced(Vector2I cell, int cost)
    {
        points -= cost;
        EmitSignal(SignalName.PointsUpdated, points);
    }

    private void OnPointsAwarded(int amount)
    {
        points += amount;
        EmitSignal(SignalName.PointsUpdated, points);
    }

    private void OnWaveEnded()
    {
        waveNumber++;
        EmitSignal(SignalName.WaveIncreased, waveNumber);
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

    public void OnStartWaveButtonPressed()
    {
        entityManager.SpawnWave(waveNumber, enemyScenes);
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
            if (points - towerScenes[previewTowerIndex].Instantiate<Tower>().cost < 0)
                return;

            Tower tower = previewTower;
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
            if (points - towerScenes[previewTowerIndex].Instantiate<Tower>().cost < 0)
                return;
            Tower tower = (Tower)previewTower.Duplicate();
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
        Tower tower = towerScenes[index].Instantiate<Tower>();
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

    public void ResetGame()
    {
        waveNumber = startingWave;
        points = startingPoints;
        life = startingLife;

        EmitSignal(SignalName.WaveIncreased, waveNumber);
        EmitSignal(SignalName.PointsUpdated, points);
        EmitSignal(SignalName.LifeUpdated, life);
    }
}

