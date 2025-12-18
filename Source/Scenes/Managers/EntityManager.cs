using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class EntityManager : Node
{
    [Signal]
    public delegate void WaveEndedEventHandler();
	[Signal]
	public delegate void PointsAwardedEventHandler(int points);
	[Signal]
	public delegate void RelayEnemyRequestNewPathEventHandler(EnemyBase enemy);
    [Signal]
	public delegate void TowerSelectedEventHandler(TowerBase tower);
	[Signal]
	public delegate void TowerPlacedEventHandler(Vector2I cell, int cost);

    private TowerBase selectedTower;
	private Node towerParent;
	public Dictionary<Vector2I, TowerBase> towers { get; private set; } = [];
	private Node enemyParent;
	public Array<EnemyBase> enemies { get; private set; } = [];
	private bool bPlacedTowerThisFrame = false;
	private Array<Vector2> enemyPath;

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

		// Entry Point
		entryPoint = entryPointScene.Instantiate<Node2D>();
		AddChild(entryPoint);
		entryPoint.Position = new Vector2(0, -1075);
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

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (enemies.Count > 0)
		{
            MoveEnemies(delta);
			RemoveEnemiesQueuedForDeletion();
		}
    }

    private void MoveEnemies(double delta)
    {
        foreach (EnemyBase enemy in enemies)
        {
			if (enemy != null && !enemy.IsQueuedForDeletion())
                enemy.MoveAlongPath(delta);
        }
    }

	private void RemoveEnemiesQueuedForDeletion()
	{
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null || enemies[i].IsQueuedForDeletion())
            {
                enemies.RemoveAt(i);
            }
        }

        if (enemies.Count == 0)
        {
            EmitSignal(SignalName.WaveEnded);
        }
    }

    public void OnPlaceTower(Vector2I cellPosition, TowerBase tower)
	{
		towerParent.AddChild(tower);
		towers[cellPosition] = tower;
		tower.Name = tower.towerName;
		tower.SetupTower(cellPosition, true);
		bPlacedTowerThisFrame = true;
		EmitSignal(SignalName.TowerPlaced, cellPosition, tower.cost);

        foreach (EnemyBase enemy in enemies)
        {
			enemy.EnemyMovedCells += tower.OnEnemyMovedCells;
            enemy.EnemyDestroyed += tower.RemoveEnemyInRange;
        }
    }

	public Node2D GetEntityAtPosition(Vector2I cellPosition)
	{
		return null;
	}

	public void OnGridCellClicked(Vector2I cellPosition)
	{
		if (!bPlacedTowerThisFrame)
		{
			if (towers.ContainsKey(cellPosition))
			{
				selectedTower = towers[cellPosition];
				EmitSignal(SignalName.TowerSelected, selectedTower);
			}
			else
			{
				selectedTower = null;
				EmitSignal(SignalName.TowerSelected, new TowerBase());
			}
		}
		else
			bPlacedTowerThisFrame = false;
	}

    public async void SpawnWave(int wave, Array<PackedScene> enemyScenes)
	{
		int waveValue = wave * 5;
		while (waveValue > 0)
		{
			waveValue -= SpawnEnemy(enemyScenes[0]);
            await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
        }
	}

	private int SpawnEnemy(PackedScene enemyScene)
	{
		EnemyBase enemy = enemyScene.Instantiate<EnemyBase>();
		enemyParent.AddChild(enemy);
        enemies.Add(enemy);

		enemy.Position = entryPoint.Position;
		enemy.path = enemyPath;
		enemy.EnemyDestroyed += OnEnemyDestroyed;
        enemy.EnemyReachedObjective += OnEnemyReachedObjective;
        foreach (var tower in towers)
        {
            enemy.EnemyMovedCells += tower.Value.OnEnemyMovedCells;
			enemy.EnemyDestroyed += tower.Value.RemoveEnemyInRange;
        }

        return enemy.value;
	}

	private void OnEnemyReachedObjective(EnemyBase enemy)
    {
        // Damage the player here

        enemy.QueueFree();
    }

    private void OnEnemyDestroyed(EnemyBase enemy)
	{
		EmitSignal(SignalName.PointsAwarded, enemy.value);
    }

    public void OnWorldPathUpdated(Array<Vector2> worldPath)
    {
        enemyPath = worldPath;
        foreach (EnemyBase enemy in enemies)
		{
            enemy.SetPath(worldPath);
        }
    }

	public void OnEnemyRequestNewPath(EnemyBase enemy)
    {
        EmitSignal(SignalName.RelayEnemyRequestNewPath, enemy);
    }
}