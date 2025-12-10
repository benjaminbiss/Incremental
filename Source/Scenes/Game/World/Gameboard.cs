using Godot;

public partial class Gameboard : Node
{
	private const int GRID_SIZE = 32;
	private Vector2I mousePosition;
	private Vector2I lastMousePosition;

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
		for (int y = -GRID_SIZE; y < GRID_SIZE; y++)
		{
			for (int x = -GRID_SIZE; x < GRID_SIZE; x++)
			{
                gridLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0,0));
            }
		}
	}

    public override void _Process(double delta)
    {
        base._Process(delta);

        lastMousePosition = mousePosition;
        mousePosition = gridLayer.LocalToMap(gridLayer.GetLocalMousePosition());

		if (lastMousePosition != mousePosition)
		{
            highlightLayer.SetCell(lastMousePosition);
            highlightLayer.SetCell(mousePosition, 0, new Vector2I(1, 0));
        }
    }
}