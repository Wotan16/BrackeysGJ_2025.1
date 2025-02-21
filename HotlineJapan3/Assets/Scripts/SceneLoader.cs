using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    //Scene enum value should have same name as game scene it represents!!!
    public enum Scene
    {
        MainMenuScene,
        ActualLevel,
        VictoryScene,
    }

    public static Scene ActiveScene { get { return targetScene; } }
    private static Scene targetScene;

    public static void Load(Scene targetScene, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SceneLoader.targetScene = targetScene;
        SceneManager.LoadScene(targetScene.ToString(), loadSceneMode);
    }

    public static void ReloadCurrentScene()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
    