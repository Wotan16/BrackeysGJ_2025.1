using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField]
    private bool showEnemyGizmos;
    public static bool ShowEnemyGizmos { get; private set; }
    [Range(0, 1)]
    [SerializeField] private float timeScale = 1;

    private void OnValidate()
    {
        ShowEnemyGizmos = showEnemyGizmos;
        Time.timeScale = timeScale;
    }
}
