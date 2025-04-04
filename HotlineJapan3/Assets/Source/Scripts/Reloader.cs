using UnityEngine;

public class Reloader : MonoBehaviour
{
    [SerializeField] private SceneLoader.Scene scene;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneLoader.Load(SceneLoader.ActiveScene);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneLoader.Load(scene);
        }
    }
}
