using Godot;

public partial class Main : Node
{
    private CanvasLayer canvasLayer;
    private SaveManager saveManager;
    private Timer autoSaveTimer;

    private int saveDataIndex = -1;
    private SaveData saveData
    {
        get { return saveManager.Saves[saveDataIndex]; }
        set { saveManager.Saves[saveDataIndex] = value; }
    }


    [Export]
    private PackedScene menuManagerScene;
    private MenuManager menuManager;

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

        canvasLayer = GetNodeOrNull<CanvasLayer>("CanvasLayer");
        result = result == true ? CheckResource(canvasLayer, "CanvasLayer") : result;

        saveManager = (SaveManager)GetNode("/root/SaveManager");
        result = result == true ? CheckResource(saveManager, "SaveManager") : result;

        menuManager = menuManagerScene.Instantiate<MenuManager>();
        canvasLayer.AddChild(menuManager);
        check = CheckResource(menuManager, "MenuManager");
        if (check)
        {
            menuManager.StartGame += OnStartGame;
            menuManager.OnPauseGame += OnPauseRequested;
            menuManager.QuitGame += OnQuitRequested;
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

    public override void _Process(double delta)
    {
        if (saveDataIndex < 0)
            return;

        saveData.PlayTime += (float)delta;
    }

    private void OnStartGame(int saveIndex)
    {
        saveDataIndex = saveIndex;
        if (saveData == null)
            saveData = new SaveData();

        autoSaveTimer = new Timer();
        AddChild(autoSaveTimer);
        autoSaveTimer.Name = "AutoSaveTimer";

        saveManager.SaveSlot(saveDataIndex);
        autoSaveTimer.Start(saveData.AutoSaveInterval);
    }

    private void OnPauseRequested(bool isPaused)
    {
        GetTree().Paused = isPaused;
    }

    private void OnAutoSaveTimeout()
    {
        if (saveData == null)
            return;

        saveManager.SaveSlot(saveDataIndex);
        autoSaveTimer.Start(saveData.AutoSaveInterval);
    }

    private void OnQuitRequested()
    {
        if (saveDataIndex >= 0 && saveDataIndex < 3)                
            saveManager.SaveSlot(saveDataIndex);
        
        GetTree().Quit();
    }
}

