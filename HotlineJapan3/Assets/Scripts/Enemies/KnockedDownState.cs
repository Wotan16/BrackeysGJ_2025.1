using UnityEngine;

public class KnockedDownState : IState
{
    private MeleeEnemy enemy;
    private MeleeEnemyAnimator animator;
    private Rigidbody2D rigidbody2D;
    private float knockbackPower = 400f;
    private float stunDuration;
    private float stunDurationDelta;


    public KnockedDownState(MeleeEnemy enemy, MeleeEnemyAnimator animator, Rigidbody2D rigidbody2D, float stunDuration)
    {
        this.enemy = enemy;
        this.animator = animator;
        this.stunDuration = stunDuration;
        this.rigidbody2D = rigidbody2D;
    }

    public void FixedTick()
    {
        return;
    }

    public void OnEnter()
    {
        stunDurationDelta = stunDuration;
        animator.SetKnockedDown(true);

        Vector3 knockbackDirection = enemy.transform.position - PlayerController.Instance.transform.position;
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2D.AddForce(knockbackDirection.normalized * knockbackPower);
    }

    public void OnExit()
    {
        animator.SetKnockedDown(false);
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Tick()
    {
        if (stunDurationDelta <= 0)
        {
            enemy.KnockedDown = false;
            return;
        }
        
        stunDurationDelta -= Time.deltaTime;
    }
}
