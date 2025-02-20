using Pathfinding;
using System;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    private FollowerEntity follower;
    [SerializeField] private MeleeEnemyAnimator animator;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Collider2D coll;

    protected override void Start()
    {
        follower = GetComponent<FollowerEntity>();
        base.Start();
    }

    protected override void InitializeStateMachine()
    {
        EmptyState idle = new EmptyState();
        MeleeChaseState chase = new MeleeChaseState(this, destinationSetter, follower, playerTransform, animator, moveSpeed, attackRange, attackCooldown);
        DeadState dead = new DeadState(animator, coll);

        Func<bool> AgroCondition() => () => CanSeePlayer();
        Func<bool> DeathCondition() => () => IsDead;

        stateMachine.AddTransition(idle, chase, AgroCondition());
        stateMachine.AddAnyTransition(dead, DeathCondition());

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
