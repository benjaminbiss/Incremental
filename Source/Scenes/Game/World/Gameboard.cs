using Godot;

public partial class Gameboard : Node
{
	private const int GRID_SIZE = 32;

    [Export]
	private NodePath gridLayerPath;
	private TileMapLayer gridLayer;

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
                gridLayer.SetCell(new Vector2I(x, y), 1, new Vector2I(0,0));
            }
		}
	}
}

