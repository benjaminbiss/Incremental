using Godot;
using Godot.Collections;

public partial class TowerBase : Node2D
{
    private Vector2I cell;
    public Array<Vector2I> cellsInRange { get; private set; } = [];
    private bool bIsPlaced = false;

    [Export]
    public string towerName { get; private set; } = "Tower Name";
    [Export]
    private int range = 6;
    [Export]
	private NodePath towerBaseSpritePath;
    private Sprite2D towerBaseSprite;
    [Export]
    private NodePath turretSpritePath;
    private Sprite2D turretSprite;

    private Array<EnemyBase> enemiesInRange = [];

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

        towerBaseSprite = GetNodeOrNull<Sprite2D>(towerBaseSpritePath);
        result = result == true ? CheckResource(towerBaseSprite, "TowerSprite") : result;

        turretSprite = GetNodeOrNull<Sprite2D>(turretSpritePath);
        result = result == true ? CheckResource(turretSprite, "TurretSprite") : result;

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

    public void SetupTower(Vector2I towerCell, bool placed)
    {
        cell = towerCell;
        cellsInRange = GetCellsInRange(Vector2I.Zero);
        Position = new Vector2(cell.X * 32 + 16, cell.Y * 32 + 16);
        bIsPlaced = placed;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (enemiesInRange.Count > 0)
		{
            Vector2 toTarget = enemiesInRange[0].GlobalPosition - GlobalPosition;
            float angle = toTarget.Angle();
            towerBaseSprite.Rotation = angle - Mathf.Pi / 2;
        }
    }

	public void OnEnemyMovedCells(Vector2I cell, EnemyBase enemy)
	{
        if (enemiesInRange.Contains(enemy) && !cellsInRange.Contains(cell))
        {
            RemoveEnemyInRange(enemy);
        }
        else if (!enemiesInRange.Contains(enemy) && cellsInRange.Contains(cell))
        {
            AddEnemyInRange(enemy);
        }
    }

	private void AddEnemyInRange(EnemyBase enemy)
    {
        if (!enemiesInRange.Contains(enemy))
            enemiesInRange.Add(enemy);
    }

    private void RemoveEnemyInRange(EnemyBase enemy)
    {
        if (enemiesInRange.Contains(enemy))
            enemiesInRange.Remove(enemy);
    }

    private Array<Vector2I> GetCellsInRange(Vector2I location)
    {
        Vector2I center = location == Vector2I.Zero ? cell : location;
        Array<Vector2I> cellsInRange = new Array<Vector2I>();

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector2I offset = new Vector2I(x, y);
                if (offset.Length() <= range)
                {
                    cellsInRange.Add(cell + offset);
                }
            }
        }
        return cellsInRange;
    }

    public Texture2D GetSpriteTexture()
    {
        towerBaseSprite = GetNodeOrNull<Sprite2D>(towerBaseSpritePath);
        if (towerBaseSprite == null)
            return null;
        return towerBaseSprite.Texture;
    }
}

