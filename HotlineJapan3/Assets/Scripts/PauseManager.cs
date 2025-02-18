using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;

    private bool isPaused = false;
    public bool IsPaused { get { return isPaused; } }

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeIsPaused();
        }
    }

    private void InitializeSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one " + GetType().Name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public static void PauseGame()
    {
        Instance.isPaused = true;
        Instance.OnGamePaused?.Invoke(Instance, EventArgs.Empty);
        TimeScaler.SetIsPaused(Instance.IsPaused);
    }

    public static void ResumeGame()
    {
        Instance.isPaused = false;
        Instance.OnGameResumed?.Invoke(Instance, EventArgs.Empty);
        TimeScaler.SetIsPaused(Instance.IsPaused);
    }

    public static void ChangeIsPaused()
    {
        if (Instance.IsPaused)
            ResumeGame();
        else
            PauseGame();
    }
}
