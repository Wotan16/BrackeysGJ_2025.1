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

    private static Scene targetScene;

    public static void Load(Scene targetScene, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SceneLoader.targetScene = targetScene;
        SceneManager.LoadScene(targetScene.ToString(), loadSceneMode);
    }
}
