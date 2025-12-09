using System;

[Serializable]
public class SaveData
{
    public string SaveName { get; set; } = "New Save";    
    public float PlayTime { get; set; } = 0f;
    public float AutoSaveInterval { get; set; } = 300f; // in seconds
}
