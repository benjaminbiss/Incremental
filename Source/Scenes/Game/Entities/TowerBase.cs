using Godot;
using Godot.Collections;

public partial class TowerBase : Node2D
{
    private Vector2I cell;
    public Array<Vector2I> cellsInRange { get; private set; } = [];
    private bool bIsPlaced = false;
    private bool bCanFire = false;

    [Export]
    public string towerName { get; private set; } = "Tower Name";
    [Export]
    public int cost { get; private set; } = 1;
    [Export]
    private int range = 1;
    [Export]
    private float damage = 1;
    [Export]
    private float rateOfFire_perSecond = 1f;

    [Export]
	private NodePath towerBaseSpritePath;
    private Sprite2D towerBaseSprite;
    [Export]
    private NodePath turretSpritePath;
    private Sprite2D turretSprite;

    private Array<EnemyBase> enemiesInRange = [];
    private EnemyBase target;

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
        Position = new Vector2(cell.X * 32, cell.Y * 32 + 16);
        bIsPlaced = placed;
        bCanFire = placed;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (enemiesInRange.Count > 0)
        {
            SetTarget();
            FaceEnemy();
            ShootAtEnemy();
        }
    }

    private void SetTarget()
    {
        target = enemiesInRange[0];
    }

    private void FaceEnemy()
    {
        Vector2 toTarget = target.GlobalPosition - GlobalPosition;
        turretSprite.Rotation = toTarget.Angle();
    }

    private void ShootAtEnemy()
    {
        if (bCanFire)
        {
            target.TakeDamage(damage);
            bCanFire = false;

            GetTree().CreateTimer(1f / rateOfFire_perSecond).Timeout += () =>
            {
                bCanFire = true;
            };
        }
    }

    public void OnEnemyMovedCells(EnemyBase enemy, Vector2I enemyCell)
	{
        if (enemiesInRange.Contains(enemy) && !cellsInRange.Contains(enemyCell))
        {
            RemoveEnemyInRange(enemy);
        }
        else if (!enemiesInRange.Contains(enemy) && cellsInRange.Contains(enemyCell))
        {
            AddEnemyInRange(enemy);
        }
    }

	private void AddEnemyInRange(EnemyBase enemy)
    {
        if (!enemiesInRange.Contains(enemy))
            enemiesInRange.Add(enemy);
    }

    public void RemoveEnemyInRange(EnemyBase enemy)
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

