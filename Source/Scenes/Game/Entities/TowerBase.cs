using Godot;
using Godot.Collections;

public partial class TowerBase : Node2D
{
	[Export]
	public string towerName { get; private set; } = "Tower Name";
    private int range = 6;
    private Vector2I cell;
    public Array<Vector2I> cellsInRange { get; private set; } = [];
    private bool bIsPlaced = false;

    [Export]
	private NodePath towerSpritePath;
    private Sprite2D towerSprite;

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
		bool check;

        towerSprite = GetNodeOrNull<Sprite2D>(towerSpritePath);
        check = CheckResource(towerSprite, "TowerSprite");
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
            towerSprite.Rotation = angle - Mathf.Pi / 2;
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
        towerSprite = GetNodeOrNull<Sprite2D>(towerSpritePath);
        if (towerSprite == null)
            return null;
        return towerSprite.Texture;
    }
}

