using Godot;

public partial class SettingsMenu : Control
{
    [Signal]
    public delegate void BackButtonPressedEventHandler();

    [Export]
    private NodePath backButtonPath;
    private Button backButton;
    
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

        backButton = GetNodeOrNull<Button>(backButtonPath);
        check = CheckResource(backButton, "BackButton");
        if (check)
            backButton.Pressed += OnBackButtonPressed;
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

    private void OnBackButtonPressed()
    {
        EmitSignal(SignalName.BackButtonPressed);
    }
}

