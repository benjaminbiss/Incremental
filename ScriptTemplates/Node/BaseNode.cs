// meta-name: Base Node (C#)
// meta-description: Starter template for Node (C#)
// meta-default: true
// meta-inherits: Node
// meta-language: CSharp

using Godot;

public partial class _CLASS_ : Node
{
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
        //bool check;
        //check = CheckResource(resource, "ResourceName");
        //if (check)
        //    resource.Pressed += OnResource;
        //result = result == true ? check : result;

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
}
