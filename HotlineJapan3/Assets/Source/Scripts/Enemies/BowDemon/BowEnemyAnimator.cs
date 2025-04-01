using UnityEngine;

public class BowEnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private const string MOVING_BOOL = "Moving";
    private const string AIMING_BOOL = "Aiming";
    private const string DEATH_TRIGGER = "Death";

    public void Die()
    {
        animator.SetTrigger(DEATH_TRIGGER);
    }

    public void SetAiming(bool aiming)
    {
        animator.SetBool(AIMING_BOOL, aiming);
    }

    public void SetMoving(bool moving)
    {
        animator.SetBool(MOVING_BOOL, moving);
    }
}
