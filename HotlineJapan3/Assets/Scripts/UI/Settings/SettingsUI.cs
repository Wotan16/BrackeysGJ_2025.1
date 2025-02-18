using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsUI : WindowUI
{
    public static SettingsUI Instance { get; private set; }

    [SerializeField]
    private Button backToPauseMenuButton;

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
        backToPauseMenuButton.onClick.AddListener(() =>
        {
            PauseUIManager.Instance.UpdateUI(PauseUIManager.PauseWindowType.Menu);
        });
    }
}
