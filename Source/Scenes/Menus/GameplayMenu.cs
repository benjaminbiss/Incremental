using Godot;
using Godot.Collections;

public partial class GameplayMenu : Control
{
    [Signal]
    public delegate void TowerButtonPressedEventHandler(int index);
    [Signal]
    public delegate void StartWaveButtonPressedEventHandler();

    [Export]
    private NodePath startWaveButtonPath;
    private Button startWaveButton;
    [Export]
    private NodePath towerButtonsContainerPath;
    private HBoxContainer towerButtonsContainer;
    [Export]
    private NodePath pointsLabelPath;
    private Label pointsLabel;
    [Export]
    private NodePath waveCounterLabelPath;
    private Label waveCounterLabel;
    [Export]
    private NodePath lifeLabelPath;
    private Label lifeLabel;

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

        // Start Wave Button
        startWaveButton = GetNodeOrNull<Button>(startWaveButtonPath);
        check = CheckResource(startWaveButton, "StartWaveButton");
        if (check)
        {
            startWaveButton.Pressed += OnStartWaveButtonPressed;
        }
        result = result == true ? check : result;

        // Tower Buttons Container
        towerButtonsContainer = GetNodeOrNull<HBoxContainer>(towerButtonsContainerPath);
        result = result == true ? CheckResource(towerButtonsContainer, "TowerButtonsContainer") : result;

        // Points Label
        pointsLabel = GetNodeOrNull<Label>(pointsLabelPath);
        result = result == true ? CheckResource(pointsLabel, "PointsLabel") : result;

        // Wave Counter Label
        waveCounterLabel = GetNodeOrNull<Label>(waveCounterLabelPath);
        result = result == true ? CheckResource(waveCounterLabel, "WaveCounterLabel") : result;

        // Life Label
        lifeLabel = GetNodeOrNull<Label>(lifeLabelPath);
        result = result == true ? CheckResource(lifeLabel, "LifeLabel") : result;


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
            
            Tower tower = towerScenes[i].Instantiate<Tower>();
            towerButton.Text = tower.towerName;
            towerButton.Name = tower.towerName + "_Button";
            AtlasTexture texture = new AtlasTexture();
            texture.Atlas = tower.GetSpriteTexture();
            texture.Region = tower.GetSpriteRegion();
            towerButton.Icon = texture;

            int index = i;
            towerButton.Pressed += () => OnTowerButtonPressed(index);
        }
    }

    private void OnTowerButtonPressed(int index)
    {
        EmitSignal(SignalName.TowerButtonPressed, index);
    }

    private void OnStartWaveButtonPressed()
    {
        EmitSignal(SignalName.StartWaveButtonPressed);
        startWaveButton.Hide();
    }

    public void OnWaveEnded()
    {
        startWaveButton.Show();
    }

    public void OnPointsUpdated(int points)
    {
        pointsLabel.Text = points.ToString();
    }

    public void OnWaveIncreased(int waveNumber)
    {
        waveCounterLabel.Text = waveNumber.ToString();
    }

    public void OnLifeUpdated(int life)
    {
        lifeLabel.Text = life.ToString();
    }
}

