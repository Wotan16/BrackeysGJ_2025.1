using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField]
    private bool showEnemyAgroRange;
    [SerializeField]
    private bool showEnemyVision;
    public static bool ShowEnemyAgroRange { get; private set; }
    public static bool ShowEnemyVision { get; private set; }
    [Range(0, 1)]
    [SerializeField] private float timeScale = 1;

    private void OnValidate()
    {
        ShowEnemyAgroRange = showEnemyAgroRange;
        ShowEnemyVision = showEnemyVision;
        Time.timeScale = timeScale;
    }
}
