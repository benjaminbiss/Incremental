using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    // SAVE INFORMATION
    public string SaveName { get; set; } = "New Save";
    public float PlayTime { get; set; } = 0f;

    // GAME DATA
    public Dictionary<string, Dictionary<string, float>> GameData { get; set; } = new Dictionary<string, Dictionary<string, float>>()
    {
        { "Resources", new Dictionary<string, float>()
            {
                { "CPU Cycles", 0.0f }, // Main currency for building towers.        
                { "RAM Blocks", 0.0f }, // Secondary currency for unlocking upgrades.                
            }
        }
    };
}
