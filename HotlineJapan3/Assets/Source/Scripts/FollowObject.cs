using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool rotateSelfWithTarget;
    [SerializeField] private bool rotateOffsetWithTarget;
    [SerializeField] private Vector2 offset;

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 currentOffset = rotateOffsetWithTarget ? target.rotation * offset : offset;
        Vector3 newPosition = target.position + currentOffset;
        transform.position = newPosition;

        if (rotateSelfWithTarget)
            transform.rotation = target.rotation;
    }

    private void OnValidate()
    {
        FollowTarget(); 
    }
}
