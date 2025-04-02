using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraTarget : MonoBehaviour
{
    [SerializeField] private float maxRange;
    [SerializeField] private Transform cameraTarget;
    public Transform CameraTarget { get { return cameraTarget; } }
    private Vector2 mouseScreenPosition;

    private void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector3 vectorToMousePos = mouseWorldPos - transform.position;
        float distance = Mathf.Clamp(vectorToMousePos.magnitude, 0, maxRange);
        cameraTarget.position = transform.position + vectorToMousePos.normalized * distance;
    }

    #region InputEvents

    public void OnLook_Input(InputAction.CallbackContext context)
    {
        mouseScreenPosition = context.ReadValue<Vector2>();
    }

    #endregion

    //Vector3 vectorToCamera = cameraTransform.position - transform.position;
    //float distance = Mathf.Clamp(vectorToCamera.magnitude, 0, maxRange);
    //transform.position = vectorToCamera.normalized* distance;
}
