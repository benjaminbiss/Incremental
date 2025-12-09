using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class SettingsOption_Slider : Control
{
	[Export]
	private NodePath nameLabelPath;
    public Label nameLabel { get; private set; }
	[Export]
	private NodePath sliderPath;
    public HSlider slider { get; private set; }
    [Export]
	private NodePath valueLabelPath;
	public LineEdit valueLabel { get; private set; }
    private string previousValidText = "";

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

		nameLabel = GetNodeOrNull<Label>(nameLabelPath);
		result = result == true ? CheckResource(nameLabel, "Name") : result;

        slider = GetNodeOrNull<HSlider>(sliderPath);
        check = CheckResource(slider, "Slider");
        if (check)
            slider.ValueChanged += OnSliderValueChanged;
        result = result == true ? check : result;

		valueLabel = GetNodeOrNull<LineEdit>(valueLabelPath);
        check = CheckResource(valueLabel, "Value");
        if (check)
            valueLabel.FocusExited += OnTextValueChanged;
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
	public void SetupOption(string newName, float newValue, float min, float max)
	{
		SetupName(newName);
		SetupRange(min, max);
		SetupValue(newValue);
    }

    private void SetupName(string newName)
	{
		if (nameLabel != null)
		{
			nameLabel.Text = newName;
		}
    }

    private void SetupRange(float min, float max)
	{
		if (slider != null)
		{
			slider.MinValue = min;
			slider.MaxValue = max;
        }
    }

    private void SetupValue(float newValue)
	{
		if (valueLabel != null && slider != null)
		{
			slider.Value = newValue;
			valueLabel.Text = $"{newValue}";
        }
    }

    private void OnSliderValueChanged(double value) 
    {
        if (this.valueLabel != null)
        {
            this.valueLabel.Text = $"{(int)value}";
        }
    }

	private void OnTextValueChanged() 
	{
		if (slider != null)
		{			
			string value = valueLabel.Text;

            RegEx regex = RegEx.CreateFromString("");
            // Allow only numbers and one decimal point
            regex.Compile(@"^[0-9]*\.?[0-9]*$");
            string filteredValue = value.Trim();
			RegExMatch result = regex.Search(filteredValue);
            // Check regex if did not match, revert to previous valid text
            if (result == null)
			{
                valueLabel.Text = slider.Value.ToString();
				return;
			}

            filteredValue = result.GetString();

			// Prevent infinite loop by only setting text if it actually changed
            if (filteredValue != previousValidText)
			{
                previousValidText = filteredValue;
				if (int.TryParse(filteredValue, out int intValue))
				{
					slider.Value = intValue;
				}
			}
		}
    }
}

