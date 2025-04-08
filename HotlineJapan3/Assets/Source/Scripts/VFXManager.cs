using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance {  get; private set; }
    public GameObject parryVFXPrefab;

    private void Awake()
    {
        InitializeSingleton();
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

    public static GameObject CreateParryVFX(Vector2 position, Quaternion rotation)
    {
        GameObject vfxObject = Instantiate(Instance.parryVFXPrefab, position, rotation);
        return vfxObject;
    }
}
