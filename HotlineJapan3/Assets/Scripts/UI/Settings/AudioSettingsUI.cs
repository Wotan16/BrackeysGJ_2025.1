using UnityEngine;

public class AudioSettingsUI : WindowUI
{
    [SerializeField]
    private SettingSliderUI soundSlider;
    [SerializeField]
    private SettingSliderUI musicSlider;

    private void Start()
    {
        soundSlider.InitializeSlider(AudioSettings.soundVolume);
        musicSlider.InitializeSlider(AudioSettings.musicVolume);
    }
}
