using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : WindowUI
{
    [SerializeField]
    private Button backToGameButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button mainMenuButton;

    private void Awake()
    {
        backToGameButton.onClick.AddListener(() =>
        {
            PauseManager.ResumeGame();
        });
        settingsButton.onClick.AddListener(() =>
        {
            PauseUIManager.Instance.UpdateUI(PauseUIManager.PauseWindowType.Settings);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
        });
    }
}
