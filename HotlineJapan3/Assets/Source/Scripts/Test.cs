using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneLoader.Load(SceneLoader.Scene.TestScene);
        }
    }
}
