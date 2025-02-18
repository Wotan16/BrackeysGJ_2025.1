using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingSliderUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI valueText;

    private SliderSetting setting;

    public void InitializeSlider(SliderSetting setting)
    {
        this.setting = setting;

        slider.minValue = setting.MinValue;
        slider.maxValue = setting.MaxValue;
        slider.value = setting.Value;
        UpdateText(setting.GetDisplayedValue());

        setting.OnValueChanged += SliderSetting_OnValueChanged;
        slider.onValueChanged.AddListener((float value) =>
        {
            setting.Value = slider.value;
        });
    }

    private void SliderSetting_OnValueChanged(float value)
    {
        UpdateText(setting.GetDisplayedValue());
    }

    public void UpdateText(float value)
    {
        valueText.text = value.ToString();
    }

    private void OnDestroy()
    {
        if (setting == null)
            return;

        setting.OnValueChanged -= SliderSetting_OnValueChanged;
    }
}
