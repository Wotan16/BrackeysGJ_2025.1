using System;
using UnityEngine;

public static class AudioSettings
{
    public static SliderSetting soundVolume = new SliderSetting(0f, 1f, 0.6f, 100f);
    public static SliderSetting musicVolume = new SliderSetting(0f, 1f, 0.5f, 100f);
}
