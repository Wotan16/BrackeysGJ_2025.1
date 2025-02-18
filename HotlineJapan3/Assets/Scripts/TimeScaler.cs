using System;
using UnityEngine;

public static class TimeScaler
{
    public static bool IsLoading {  get; private set; } = false;
    public static bool IsPaused { get; private set; } = false;

    public static void SetIsLoading(bool value)
    {
        IsLoading = value;
        UpdateTimeScale();
    }

    public static void SetIsPaused(bool value)
    {
        IsPaused = value;
        UpdateTimeScale();
    }

    private static void UpdateTimeScale()
    {
        Time.timeScale = IsLoading || IsPaused ? 0f : 1f;
    }
}
