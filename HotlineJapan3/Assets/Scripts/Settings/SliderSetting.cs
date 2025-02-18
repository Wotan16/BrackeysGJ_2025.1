using System;
using UnityEngine;

[Serializable]
public class SliderSetting
{
    public event Action<float> OnValueChanged;

    private float value;
    private float minValue;
    private float maxValue;
    public float MinValue => minValue;
    public float MaxValue => maxValue;

    private float displayedValueMultiplier;
    public int displayedRoundTo;
    

    public SliderSetting(float minValue, float maxValue, float defaultValue, float displayedValueMultiplier = 1f, int displayedRoundTo = 0)
    {
        value = defaultValue;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.displayedValueMultiplier = displayedValueMultiplier;
        this.displayedRoundTo = displayedRoundTo;
    }

    public float Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            OnValueChanged?.Invoke(this.value);
        }
    }

    public float GetDisplayedValue()
    {
        return GetDisplayedValue(value);
    }

    public float GetDisplayedValue(float value)
    {
        float displayedValue = (float)Math.Round(value * displayedValueMultiplier, displayedRoundTo);
        return displayedValue;
    }
}
