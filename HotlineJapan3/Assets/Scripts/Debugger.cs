using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField]
    private bool showEnemyGizmos;
    public static bool ShowEnemyGizmos { get; private set; }

    private void OnValidate()
    {
        ShowEnemyGizmos = showEnemyGizmos;
    }
}
