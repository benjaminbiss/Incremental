using Godot;
using Godot.Collections;
using System;

public partial class EnemyBase : Node2D
{
    [Signal]
	public delegate void RequestNewPathEventHandler(EnemyBase enemy);
    [Signal]
	public delegate void EnemyDestroyedEventHandler(EnemyBase enemy);
	[Signal]
    public delegate void EnemyReachedObjectiveEventHandler(EnemyBase enemy);
	[Signal]
	public delegate void EnemyMovedCellsEventHandler(EnemyBase enemy, Vector2I cell);

    public Array<Vector2> path = [];
	private int currentPathIndex = 0;

    [Export]
	public int value { get; private set; } = 1;
	[Export]
	public float health { get; private set; } = 10;
	[Export]
	public float speed_tilePerSecond { get; private set; } = 3f;

    [Export]
	private NodePath healthComponentPath;
	private Health healthComponent;


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

		healthComponent = GetNodeOrNull<Health>(healthComponentPath);
		check = CheckResource(healthComponent, "ResourceName");
		if (check)
		{
			healthComponent.Setup(health);
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

    public void MoveAlongPath(double delta)
    {
        Position = Position.MoveToward(path[currentPathIndex + 1], speed_tilePerSecond * 32f * (float)delta);
        Vector2I enemyCell = new Vector2I(Mathf.FloorToInt(Position.X / 32), Mathf.FloorToInt(Position.Y / 32));
        if (enemyCell.Y == 0)
        {
            EmitSignal(SignalName.EnemyReachedObjective, this);
			return;
        }
		else
		{
            EmitSignal(SignalName.EnemyMovedCells, this, enemyCell);
        }

		if (Position.DistanceTo(path[currentPathIndex + 1]) < 1f)
            currentPathIndex++;
    }

	private void ReachedObjective()
    {
        EmitSignal(SignalName.EnemyReachedObjective, this);
        QueueFree();
    }

    public void SetPath(Array<Vector2> worldPath)
    {
		if (path.Count == 0)
		{
            path = worldPath;
            return;
        }
		else if (path[currentPathIndex] == worldPath[currentPathIndex] && path[currentPathIndex + 1] == worldPath[currentPathIndex + 1])
			path = worldPath;
		else
			EmitSignal(SignalName.RequestNewPath, this);
    }

    public void TakeDamage(float amount)
    {
        float remainingHealth = healthComponent.TakeDamage(amount);
        if (remainingHealth <= 0)
        {
            EmitSignal(SignalName.EnemyDestroyed, this);
            QueueFree();
        }
    }

	public float GetCurrentHealth()
    {
        return healthComponent.currentHealth;
    }
}

