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
        { "World", new Dictionary<string, float>()
            {                
                { "BaseSize", 10.0f },
                { "Additive_Size", 0.0f },
                { "Multiplicative_Size", 1.0f },
            }
        },
        { "Player", new Dictionary<string, float>()
            {
                { "BaseHealth", 10.0f },
                { "Additive_Health", 0.0f },
                { "Multiplicative_Health", 1.0f },
            }
        },
        { "Towers", new Dictionary<string, float>()
            {
                { "BaseDamage", 1.0f },
                { "Additive_Damage", 0.0f },
                { "Multiplicative_Damage", 1.0f },
            }
        },
        { "Enemies", new Dictionary<string, float>()
            {
                { "BaseHealth", 1.0f },
                { "Additive_Health", 0.0f },
                { "Multiplicative_Health", 1.0f },
            }
        },
        { "Resources", new Dictionary<string, float>()
            {
                { "CPU Cycles", 0.0f }, // Main currency for building towers.        
                { "RAM Blocks", 0.0f }, // Secondary currency for unlocking upgrades.                
            }
        }
    };
}
