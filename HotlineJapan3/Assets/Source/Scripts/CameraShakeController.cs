using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    public static CameraShakeController Instance { get; private set; }

    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float defaultShakeForce;

    private void Awake()
    {
        InitializeSingleton();
        EnemyBase.OnAnyEnemyDead += EnemyBase_OnAnyEnemyDead;
    }

    private void EnemyBase_OnAnyEnemyDead(EnemyBase obj)
    {
        ShakeCamera();
    }

    private void InitializeSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one " + GetType().Name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public static void ShakeCamera()
    {
        Instance.impulseSource.GenerateImpulseWithForce(Instance.defaultShakeForce);
    }

    private void OnDestroy()
    {
        EnemyBase.OnAnyEnemyDead -= EnemyBase_OnAnyEnemyDead;
    }
}
