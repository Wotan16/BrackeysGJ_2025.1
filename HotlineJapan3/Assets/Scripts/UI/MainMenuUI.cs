using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button toSettingsButton;
    [SerializeField] private Button fromSettingsButton;
    [SerializeField] private GameObject settingsObject;
    [SerializeField] private GameObject menuObject;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneLoader.Load(SceneLoader.Scene.ActualLevel);
        });

        toSettingsButton.onClick.AddListener(() =>
        {
            settingsObject.SetActive(true);
            menuObject.SetActive(false);
        });

        fromSettingsButton.onClick.AddListener(() =>
        {
            settingsObject.SetActive(false);
            menuObject.SetActive(true);
        });

        AudioManager.PlayMusic(MusicType.MainMenu);
        TimeScaler.SetIsLoading(false);
        TimeScaler.SetIsPaused(false);
    }
}
