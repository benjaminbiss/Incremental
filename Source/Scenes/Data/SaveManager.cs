using Godot;
using System.Text.Json;
using System.IO;

public partial class SaveManager : Node
{
    private const int SAVE_SLOT_COUNT = 3;
    public SaveData[] Saves { get; private set; } = new SaveData[SAVE_SLOT_COUNT];

    private string SavePath(int slot) => $"user://Save{slot}.json";

    public override void _Ready()
    {
        LoadAllSaves();
    }

    public void LoadAllSaves()
    {
        for (int i = 0; i < SAVE_SLOT_COUNT; i++)
        {
            string path = SavePath(i);
            if (File.Exists(ProjectSettings.GlobalizePath(path)))
            {
                string json = File.ReadAllText(ProjectSettings.GlobalizePath(path));
                Saves[i] = JsonSerializer.Deserialize<SaveData>(json);
            }
        }
    }

    public void SaveSlot(int slot)
    {
        if (slot < 0 || slot >= SAVE_SLOT_COUNT) 
            return;

        string json = JsonSerializer.Serialize(Saves[slot]);
        File.WriteAllText(ProjectSettings.GlobalizePath(SavePath(slot)), json);
        GD.Print($"Saved slot {slot}");
    }
}