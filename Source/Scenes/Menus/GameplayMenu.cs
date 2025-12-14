using Godot;
using Godot.Collections;

public partial class GameplayMenu : Control
{
    [Signal]
    public delegate void TowerButtonPressedEventHandler(int index);

    [Export]
    private NodePath towerButtonsContainerPath;
    private HBoxContainer towerButtonsContainer;

    public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {GetType().Name} | Initialization failed.");
		}
	}

	private bool Initialize()
	{
        bool result = true;
        bool check;

        towerButtonsContainer = GetNodeOrNull<HBoxContainer>(towerButtonsContainerPath);
        check = CheckResource(towerButtonsContainer, "TowerButtonsContainer");
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

    public void FillTowerButtons(Array<PackedScene> towerScenes)
    {
        for (int i = 0; i < towerScenes.Count; i++)
        {
            Button towerButton = new Button();
            towerButtonsContainer.AddChild(towerButton);
            
            TowerBase tower = towerScenes[i].Instantiate<TowerBase>();
            towerButton.Text = tower.towerName;
            towerButton.Name = tower.towerName + "_Button";
            towerButton.Icon = tower.GetSpriteTexture();

            int index = i;
            towerButton.Pressed += () => OnTowerButtonPressed(index);
        }
    }

    private void OnTowerButtonPressed(int index)
    {
        EmitSignal(SignalName.TowerButtonPressed, index);
    }
}

