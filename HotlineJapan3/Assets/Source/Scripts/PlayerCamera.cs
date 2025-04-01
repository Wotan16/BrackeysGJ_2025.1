using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private CinemachineCamera CMCamera;

    private void Awake()
    {
        CMCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        CMCamera.Target.TrackingTarget = PlayerController.Instance.transform;
    }
}
