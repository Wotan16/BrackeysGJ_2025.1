using UnityEngine;

public class ShadowsController : MonoBehaviour
{
    [SerializeField] private Camera shadowsCamera;
    [SerializeField] private Transform shadowsObject;
    public float verticalSize = 9f;
    public Vector2 offset;

    private void OnValidate()
    {
        shadowsCamera.orthographicSize = verticalSize / 2;
        float xScale = 16f/9f * verticalSize;
        shadowsCamera.transform.position = new Vector3(transform.position.x, transform.position.y, shadowsCamera.transform.position.z);
        shadowsObject.transform.localScale = new Vector3(xScale, verticalSize, 1);
        shadowsObject.position = transform.position + new Vector3(offset.x, offset.y);
    }
}
