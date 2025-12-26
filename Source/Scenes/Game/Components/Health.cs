using Godot;

public partial class Health : Control
{
	public float currentHealth { get; private set; }
	private float maxHealth;

	[Export]
	private NodePath healthBarPath;
	private ProgressBar healthBar;

	public override void _Ready()
	{
		if (!Initialize())
		{
			GD.PrintErr($" {GetType().Name} | Initialization failed.");
			return;
		}
	}

	private bool Initialize()
	{
		bool result = true;
		bool check;

		healthBar = GetNodeOrNull<ProgressBar>(healthBarPath);
		check = CheckResource(healthBar, "ResourceName");
		if (check)
		{ }
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

	public void Setup(float max)
	{
		maxHealth = max;

        currentHealth = maxHealth;
        healthBar.MaxValue = maxHealth;
    }

	public float TakeDamage(float amount)
	{
		currentHealth -= amount;
		healthBar.Value = currentHealth;
		return currentHealth;
	}
}

