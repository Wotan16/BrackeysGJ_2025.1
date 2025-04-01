using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button toSettingsButton;
    [SerializeField] private Button fromSettingsButton;
    [SerializeField] private GameObject settingsObject;
    [SerializeField] private GameObject menuObject;
    [SerializeField] private BlackScreenUI blackScreenUI;
    [SerializeField] private GameObject controlsObject;
    [SerializeField] private Button okButton;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            settingsObject.SetActive(false);
            menuObject.SetActive(false);
            controlsObject.SetActive(true);
        });

        okButton.onClick.AddListener(() =>
        {
            okButton.interactable = false;
            StartCoroutine(LoadSceneAfterBlackScreen());    
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

        settingsObject.SetActive(false);
        menuObject.SetActive(true);
        controlsObject.SetActive(false);

        blackScreenUI.ToBlack();
        blackScreenUI.FromBlackOverTime(10);
    }

    private IEnumerator LoadSceneAfterBlackScreen()
    {
        blackScreenUI.ToBlackOverTime(10f);
        yield return new WaitForSeconds(2);
        SceneLoader.Load(SceneLoader.Scene.FloorOne);
    }
}
