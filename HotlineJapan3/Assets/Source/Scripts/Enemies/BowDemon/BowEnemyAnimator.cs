using System;
using UnityEngine;

public class BowEnemyAnimator : MonoBehaviour
{
    public event Action OnBowRelease;
    public event Action OnShootAnimationEnded;

    [SerializeField] private Animator animator;
    private const string MOVING_BOOL = "Moving";
    private const string AIMING_BOOL = "Aiming";
    private const string DEATH_TRIGGER = "Death";
    private const string DRAW_AND_SHOOT_TRIGGER = "Shoot";

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

    public void Shoot()
    {
        animator.SetTrigger(DRAW_AND_SHOOT_TRIGGER);
    }

    #region AnimationEvents

    public void OnBowRelease_Anim()
    {
        OnBowRelease?.Invoke();
    }

    public void OnShootAnimationEnded_Anim()
    {
        OnShootAnimationEnded?.Invoke();
    }

    #endregion
}
