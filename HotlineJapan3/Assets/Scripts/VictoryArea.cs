using UnityEngine;

public class VictoryArea : MonoBehaviour
{
    [SerializeField] private SceneLoader.Scene nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneLoader.Load(nextScene);
        }
    }
}
