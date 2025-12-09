using System;
using System.Collections.Generic;

public partial class SettingsData
{
    // SETTINGS INFORMATION
    public Dictionary<string, Dictionary<string, float>> GameSettings { get; set; } = new Dictionary<string, Dictionary<string, float>>()
    {
        { "AutoSaveInterval", new Dictionary<string, float>()
            {
                { "Value", 300.0f },
                { "Min", 30.0f },
                { "Max", 600.0f },
            }
        },
    };

    // AUDIO INFORMATION
    public Dictionary<string, Dictionary<string, float>> AudioSettings { get; set; } = new Dictionary<string, Dictionary<string, float>>()
    {
        { "MasterVolume", new Dictionary<string, float>()
            {
                { "Value", 80.0f },
                { "Min", 0.0f },
                { "Max", 100.0f },
            }
        },
        { "MusicVolume", new Dictionary<string, float>()
            {
                { "Value", 80.0f },
                { "Min", 0.0f },
                { "Max", 100.0f },
            }
        },
        { "SFXVolume", new Dictionary<string, float>()
            {
                { "Value", 80.0f },
                { "Min", 0.0f },
                { "Max", 100.0f },
            }
        },
    };

    // CONTROLS INFORMATION
    public Dictionary<string, Dictionary<string, float>> ControlsSettings { get; set; } = new Dictionary<string, Dictionary<string, float>>()
    {
        { "Sensitivity", new Dictionary<string, float>()
            {
                { "Value", 20.0f },
                { "Min", 0.0f },
                { "Max", 100.0f },
            }
        },
    };
}
