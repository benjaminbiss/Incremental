using Godot;
using Godot.Collections;

public partial class Gameboard : Node2D
{
	[Signal]
	public delegate void GridCellClickedEventHandler(Vector2I cellPosition);

    private const int GRID_SIZE_X = 12;
    private const int GRID_SIZE_Y = 32;
	public Vector2I cellUnderMousePosition { get; private set; }
	private Vector2I cellUnderLastMousePosition;
    private Vector2I tileUnderMousePosition;
    private Vector2I tileUnderLastMousePosition;

    private AStarGrid2D astarGrid = new AStarGrid2D();
    private Array<Vector2I> currentPath = [];
    private Array<Vector2> worldPath = [];
    private Vector2I pathStart = new Vector2I(0, -GRID_SIZE_Y);
    private Vector2I pathEnd = new Vector2I(0, 0);

    [Export]
	private NodePath gridLayerPath;
	private TileMapLayer gridLayer;
	[Export]
	private NodePath highlightLayerPath;
    private TileMapLayer highlightLayer;

    public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {GetType().Name} | Initialization failed.");
			return;
		}

        SetupGrid();
        SetupAStarGrid();
        SetCurrentPath(pathStart, pathEnd);
    }

	private bool Initialize()
	{
		bool result = true;

        gridLayer = GetNode<TileMapLayer>(gridLayerPath);
		result = result == true ? CheckResource(gridLayer, "Grid") : result;

        highlightLayer = GetNode<TileMapLayer>(highlightLayerPath);
        result = result == true ? CheckResource(highlightLayer, "Highlight") : result;


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

	private void SetupGrid()
	{
		for (int y = -GRID_SIZE_Y; y <= 0; y++)
		{
			for (int x = -GRID_SIZE_X; x <= GRID_SIZE_X; x++)
			{
				if (y == 0)
                    gridLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(4, 0));
                else
					gridLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
            }
		}
	}

    private void SetupAStarGrid()
    {
        astarGrid.Clear();
        astarGrid.Region = new Rect2I(new Vector2I(-GRID_SIZE_X, -GRID_SIZE_Y), new Vector2I(GRID_SIZE_X * 2 + 1, GRID_SIZE_Y + 1));
        astarGrid.CellSize = new Vector2(1, 1); 
        astarGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
        astarGrid.Update();

        SetupAStarObstacles();
    }

    private void SetupAStarObstacles()
    {
        // Mark occupied tiles as solid
        for (int y = -GRID_SIZE_Y; y <= 0; y++)
        {
            for (int x = -GRID_SIZE_X; x <= GRID_SIZE_X; x++)
            {
                Vector2I cell = new Vector2I(x, y);
                int tileId = gridLayer.GetCellSourceId(cell);
                if (tileId == -1)
                    continue;

                TileData data = gridLayer.GetCellTileData(cell);
                if (data.HasCustomData("Occupied"))
                {
                    bool isOccupied = data.GetCustomData("Occupied").AsBool();
                    astarGrid.SetPointSolid(new Vector2I(x, y), isOccupied);
                }
            }
        }

        astarGrid.Update();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        cellUnderLastMousePosition = cellUnderMousePosition;
        cellUnderMousePosition = gridLayer.LocalToMap(gridLayer.GetLocalMousePosition());

        if (cellUnderMousePosition.X >= -GRID_SIZE_X && cellUnderMousePosition.X < GRID_SIZE_X && cellUnderMousePosition.Y >= -GRID_SIZE_Y && cellUnderMousePosition.Y < 0)
        {
            if (cellUnderLastMousePosition != cellUnderMousePosition)
            {
                tileUnderLastMousePosition = tileUnderMousePosition;
                tileUnderMousePosition = highlightLayer.GetCellAtlasCoords(cellUnderMousePosition);
                highlightLayer.SetCell(cellUnderLastMousePosition, 0, tileUnderLastMousePosition);
                highlightLayer.SetCell(cellUnderMousePosition, 0, new Vector2I(1, 0));
            }
            if (Input.IsActionJustPressed("Left_Click")) 
            {
                EmitSignal(SignalName.GridCellClicked, cellUnderMousePosition);
            }
        }
		else
		{
            highlightLayer.SetCell(cellUnderLastMousePosition);
        }
    }

    private void PreviewTowerRange(Array<Vector2I> cells)
	{
        Vector2I tile = Vector2I.Zero;

        foreach (Vector2I cell in cells)
        {
            if (!astarGrid.IsInBounds(cell.X, cell.Y))
                continue;

            if (astarGrid.IsInBounds(cellUnderMousePosition.X, cellUnderMousePosition.Y) && astarGrid.IsPointSolid(cellUnderMousePosition))
                tile = new Vector2I(3, 0);
            else
                tile = new Vector2I(2, 0);

            if (cellUnderMousePosition.X >= -GRID_SIZE_X && cellUnderMousePosition.X < GRID_SIZE_X && cellUnderMousePosition.Y >= -GRID_SIZE_Y && cellUnderMousePosition.Y < 0)
            {
                highlightLayer.SetCell(cell, 0, tile);
            }
        }
    }

    private void DisplayTowerRange(Array<Vector2I> cells)
    {
        foreach (Vector2I cell in cells)
        {
            if (cellUnderMousePosition.X >= -GRID_SIZE_X && cellUnderMousePosition.X < GRID_SIZE_X && cellUnderMousePosition.Y >= -GRID_SIZE_Y && cellUnderMousePosition.Y < 0)
            {
                highlightLayer.SetCell(cell, 0, new Vector2I(2, 0));
            }
        }
    }

    public void ShowTowerPlacementPreview(Array<Vector2I> cells)
    {
        ClearHighlights();
        if (cells == null)
            return;

        PreviewTowerRange(cells);
    }

    public void OnTowerPlaced(Vector2I cell)
    {
        TileData data = gridLayer.GetCellTileData(cell);
        if (data.HasCustomData("Occupied"))
        {
            astarGrid.SetPointSolid(cell, true);
        }
        SetCurrentPath(pathStart, pathEnd);
    }

    public void OnTowerSelected(TowerBase tower)
    {
        if (tower.Position == Vector2.Zero)
            ClearHighlights();
        else
            DisplayTowerRange(tower.cellsInRange);
    }
    private void ClearHighlights()
    {
        highlightLayer.Clear();
        tileUnderLastMousePosition = Vector2I.Zero;
        tileUnderMousePosition = Vector2I.Zero;
    }

    public override void _Draw()
    {
        base._Draw();

        if (worldPath.Count == 0)
            return;

        for (int i = 0; i < worldPath.Count - 1; i++)
        {
            DrawLine(worldPath[i], worldPath[i + 1], Colors.Red, 2);
        }
    }

    private void SetCurrentPath(Vector2I start, Vector2I end)
    {
        GetShortestPath(start, end);
        QueueRedraw();
    }

    public void GetShortestPath(Vector2I start, Vector2I end)
    {
        currentPath = GetPath(start, end);
        worldPath = ConvertToWorld(currentPath);
    }

    private Array<Vector2I> GetPath(Vector2I start, Vector2I end)
    {
        return astarGrid.GetIdPath(start, end);
    }

    private Array<Vector2> ConvertToWorld(Array<Vector2I> path)
    {
        worldPath = [];
        for (int i = 0; i < path.Count; i++)
        {
            worldPath.Add(gridLayer.MapToLocal(path[i]));
        }

        return worldPath;
    }
}