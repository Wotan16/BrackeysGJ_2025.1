using System;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIManager : MonoBehaviour
{
    public static PauseUIManager Instance { get; private set; }

    public enum PauseWindowType
    {
        Disabled,
        Menu,
        Settings,
        Death
    }

    [Serializable]
    private class TypeWindowPair
    {
        public PauseWindowType type;
        public WindowUI window;
    }

    [SerializeField]
    private List<TypeWindowPair> windows;

    private void Awake()
    {
        InitializeSingleton();
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

    private void Start()
    {
        PauseManager.Instance.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.Instance.OnGameResumed += PauseManager_OnGameResumed;
        //Player.Instance.OnPlayerDead += Player_OnPlayerDead;
        
        UpdateUI(PauseWindowType.Disabled);
    }

    private void PauseManager_OnGameResumed(object sender, EventArgs e)
    {
        UpdateUI(PauseWindowType.Disabled);
    }

    private void PauseManager_OnGamePaused(object sender, EventArgs e)
    {
        UpdateUI(PauseWindowType.Menu);
    }

    private void Player_OnPlayerDead()
    {
        UpdateUI(PauseWindowType.Death);
    }


    public void UpdateUI(PauseWindowType targetType)
    {

        foreach (var pair in windows)
        {
            if(pair.type == targetType)
                pair.window.Show();
            else
                pair.window.Hide();
        }
    }
}
