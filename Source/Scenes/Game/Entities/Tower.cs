using Godot;
using Godot.Collections;

[Tool]
public partial class Tower : Sprite2D
{
    private Vector2I cell;
    public Array<Vector2I> cellsInRange { get; private set; } = [];
    private bool bIsPlaced = false;
    private bool bCanFire = false;

    public string towerName { get; private set; } = "Tower Name";
    public int cost { get; private set; } = 1;
    private int range = 1;
    private float damage = 1;
    private float rateOfFire_perSecond = 1f;

    private Array<EnemyBase> enemiesInRange = [];
    private EnemyBase target;

    private Placeables.E_TowerTypes towerType;
    [Export]
    public Placeables.E_TowerTypes TowerType
    {
        get { return towerType; }
        set { SetTowerType(value); }
    }

    private void SetTowerType(Placeables.E_TowerTypes newType)
    {
        towerType = newType;
        if (Placeables.towerAtlasRegions.ContainsKey(towerType))
        {
            RegionRect = Placeables.towerAtlasRegions[towerType];
            towerName = towerType.ToString();
            // cost
            // range
            // damage
            // rateOfFire_perSecond
        }
        else
        {
            GD.PrintErr($" Tower | Atlas region for tower type {towerType} not found.");
        }
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
        Rotation = toTarget.Angle();
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
        return Texture;
    }

    public Rect2 GetSpriteRegion()
    {
        return RegionRect;
    }
}

