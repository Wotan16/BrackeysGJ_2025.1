using Pathfinding;
using UnityEngine;

public class DeadState: IState
{
    private MeleeEnemyAnimator animator;
    private Collider2D collider;

    public DeadState(MeleeEnemyAnimator animator, Collider2D collider)
    {
        this.animator = animator;
        this.collider = collider;
    }

    public void OnEnter()
    {
        animator.Die();
        collider.enabled = false;
        return;
    }

    public void OnExit()
    {
        return;
    }

    public void Tick()
    {
        return;
    }

    public void FixedTick()
    {
        return;
    }
}
