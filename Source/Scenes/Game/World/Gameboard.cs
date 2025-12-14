using Godot;
using Godot.Collections;
using System;

public partial class Gameboard : Node
{
	[Signal]
	public delegate void GridCellClickedEventHandler(Vector2I cellPosition); 

    private const int GRID_SIZE_X = 12;
    private const int GRID_SIZE_Y = 32;
	public Vector2I cellUnderMousePosition { get; private set; }
	private Vector2I cellUnderLastMousePosition;

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
			for (int x = -GRID_SIZE_X; x < GRID_SIZE_X; x++)
			{
				if (y == 0)
                    gridLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(5, 0));
                else if(x == GRID_SIZE_X - 1 || y == -1)
                    gridLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(1,0));
                else
					gridLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
            }
		}
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
                highlightLayer.SetCell(cellUnderLastMousePosition);
                highlightLayer.SetCell(cellUnderMousePosition, 0, new Vector2I(2, 0));
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

	private void DisplayTowerRange(Array<Vector2I> cells)
	{
        foreach (Vector2I cell in cells)
        {
            highlightLayer.SetCell(cell, 0, new Vector2I(3, 0));
        }
    }

    public void OnTowerSelected(TowerBase tower)
    {
        DisplayTowerRange(tower.cellsInRange);
    }

    public void ShowTowerPlacementPreview(Array<Vector2I> cells)
    {
        highlightLayer.Clear();
        if (cells == null)
            return;

        DisplayTowerRange(cells);
    }
}