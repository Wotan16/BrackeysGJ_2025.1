using Pathfinding;
using System;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    private FollowerEntity follower;
    [SerializeField] private MeleeEnemyAnimator animator;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Collider2D coll;
    [SerializeField] private EnemySwordHitbox swordHitbox;
    [SerializeField] private float stunDuration;
    [SerializeField] private Rigidbody2D rb2D;
    public bool KnockedDown = false;

    protected override void Start()
    {
        follower = GetComponent<FollowerEntity>();
        base.Start();

        swordHitbox.OnHitPlayer += SwordHitbox_OnHitPlayer;
    }

    private void SwordHitbox_OnHitPlayer(PlayerController player)
    {
        player.TakeDamage((Vector2 parryDireciton) =>
        {
            AudioManager.PlaySound(SoundType.SwordClash);
            KnockedDown = true;
        });
    }

    protected override void InitializeStateMachine()
    {
        EmptyState idle = new EmptyState();
        MeleeChaseState chase = new MeleeChaseState(this, destinationSetter, follower, playerTransform, animator, moveSpeed, attackRange, attackCooldown, swordHitbox);
        KnockedDownState knockedDownState = new KnockedDownState(this, animator, rb2D, stunDuration);
        MeleeDeadState dead = new MeleeDeadState(animator, coll);

        Func<bool> AgroCondition() => () => CanSeePlayer();
        Func<bool> DeathCondition() => () => IsDead;
        Func<bool> KnockedDownCondition() => () => KnockedDown;
        Func<bool> GetUpCondition() => () => !KnockedDown;

        stateMachine.AddTransition(idle, chase, AgroCondition());
        stateMachine.AddTransition(knockedDownState, chase, GetUpCondition());
        stateMachine.AddAnyTransition(dead, DeathCondition());
        stateMachine.AddAnyTransition(knockedDownState, KnockedDownCondition());

        stateMachine.SetState(idle);
    }

    protected override void HealthSystem_OnDamaged()
    {
        Debug.Log("Damaged");
    }

    protected override void HealthSystem_OnDead()
    {
        Debug.Log("Dead");
    }
}
