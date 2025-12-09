using Godot;

public partial class Main : Node
{
    private CanvasLayer canvasLayer;
    private SaveManager saveManager;
    private Timer autoSaveTimer;

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
        if (saveManager.SaveDataIndex < 0)
            return;

        saveManager.SaveData.PlayTime += (float)delta;
    }

    private void OnStartGame(int saveIndex)
    {
        saveManager.SetSaveDataIndex(saveIndex);
        if (saveManager.SaveData == null)
            saveManager.SaveData = new SaveData();

        autoSaveTimer = new Timer();
        AddChild(autoSaveTimer);
        autoSaveTimer.Name = "AutoSaveTimer";

        saveManager.SaveSlot(saveManager.SaveDataIndex);
        autoSaveTimer.Start(saveManager.Settings.GameSettings["AutoSaveInterval"]["Value"]);
    }

    private void OnPauseRequested(bool isPaused)
    {
        GetTree().Paused = isPaused;
    }

    private void OnAutoSaveTimeout()
    {
        if (saveManager.SaveData == null)
            return;

        saveManager.SaveSlot(saveManager.SaveDataIndex);
        autoSaveTimer.Start(saveManager.Settings.GameSettings["AutoSaveInterval"]["Value"]);
    }

    private void OnQuitRequested()
    {
        if (saveManager.SaveDataIndex >= 0 && saveManager.SaveDataIndex < 3)                
            saveManager.SaveSlot(saveManager.SaveDataIndex);        
        
        GetTree().Quit();
    }
}

