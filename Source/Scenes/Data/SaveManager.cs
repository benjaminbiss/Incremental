using Godot;
using System.Text.Json;
using System.IO;
using System;
using System.Collections.Generic;

public partial class SaveManager : Node
{
    private const int SAVE_SLOT_COUNT = 3;
    public int SaveDataIndex { get; private set; } = -1;
    public SaveData[] Saves { get; private set; } = new SaveData[SAVE_SLOT_COUNT];
    public SaveData SaveData
    {
        get { return Saves[SaveDataIndex]; }
        set { Saves[SaveDataIndex] = value; }
    }
    public SettingsData Settings { get; private set; }

    private string SavePath(int slot) => $"user://Save{slot}.json";
    private string SettingsPath = "user://Settings.json";

    public override void _Ready()
    {
        LoadAllSaves();
        LoadSettings();
    }

    private void LoadAllSaves()
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

    public void SetSaveDataIndex(int index)
    {
        if (index < 0 || index >= SAVE_SLOT_COUNT)
        {
            GD.PrintErr("Invalid save data index.");
            return;
        }
        SaveDataIndex = index;
    }

    private void LoadSettings()
    { 
        string path = "user://Settings.json";
        if (File.Exists(ProjectSettings.GlobalizePath(path)))
        {
            string json = File.ReadAllText(ProjectSettings.GlobalizePath(path));
            Settings = JsonSerializer.Deserialize<SettingsData>(json);
        }
        else
        {
            Settings = new SettingsData();
        }
    }

    public void UpdateSettingValues(Dictionary<string, Dictionary<string, float>> settings, string name, string param, float value)
    {
        
    }

    public void SaveSettings()
    {
        string json = JsonSerializer.Serialize(Settings);
        File.WriteAllText(ProjectSettings.GlobalizePath(SettingsPath), json);
        GD.Print($"Saved Settings");
    }
}