using UnityEngine;

public class FootstepsAudio : MonoBehaviour
{
    [SerializeField] private float stepDistance = 1f;
    private float stepDelta = 0f;
    private Vector3 previousPosition = Vector3.zero;

    private void Awake()
    {
        stepDelta = stepDistance;
    }

    private void Update()
    {
        Vector3 moveVector = transform.position - previousPosition;
        previousPosition = transform.position;
        stepDelta -= moveVector.magnitude;
        if (stepDelta <= 0f)
        {
            stepDelta = stepDistance;
            AudioManager.PlaySound(SoundType.Footsteps_Wood);
        }
    }
}
