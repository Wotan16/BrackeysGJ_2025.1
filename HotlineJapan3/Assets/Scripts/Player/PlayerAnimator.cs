using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public event Action OnAttackAnimationEnded;

    [SerializeField] private Animator animator;
    private const string ATTACK_TRIGGER = "Attack";
    private const string PARRY_BOOL = "Parrying";
    private const string DASHING_BOOL = "Dashing";
    private const string MOVING_BOOL = "Moving";
    private const string DEATH_TRIGGER = "Death";

    public void Attack()
    {
        animator.SetTrigger(ATTACK_TRIGGER);
    }

    public void SetParrying(bool parrying)
    {
        animator.SetBool(PARRY_BOOL, parrying);
    }

    public void Die()
    {
        animator.SetTrigger(DEATH_TRIGGER);
    }

    public void SetDashing(bool dashing)
    {
        animator.SetBool(DASHING_BOOL, dashing);
    }

    public void SetMoving(bool moving)
    {
        animator.SetBool(MOVING_BOOL, moving);
    }

    #region AnimationEvents

    public void OnAttackAnimationEnded_Anim()
    {
        OnAttackAnimationEnded?.Invoke();
    }

    #endregion
}
