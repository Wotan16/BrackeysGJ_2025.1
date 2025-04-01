using UnityEngine;

public class BowDeadState : IState
{
    private BowEnemyAnimator animator;
    private Collider2D collider;

    public BowDeadState(BowEnemyAnimator animator, Collider2D collider)
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
