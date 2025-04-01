using NUnit.Framework;
using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BowEnemy : EnemyBase
{
    private FollowerEntity follower;

    [Header("Bow enemy params")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float timeToAim;

    [Header("Bow enemy references")]
    [SerializeField] private BowEnemyAnimator animator;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private Rigidbody2D rb2D;
    private bool canSeePlayer;

    [SerializeField] private List<Transform> partolPoints;

    protected override void Start()
    {
        follower = GetComponent<FollowerEntity>();
        base.Start();
    }

    protected override void InitializeStateMachine()
    {
        EmptyState idle = new EmptyState();
        BowChaseState chase = new BowChaseState(this, destinationSetter, follower, playerTransform, animator, moveSpeed);
        BowAimingState aiming = new BowAimingState(this, animator, timeToAim, attackCooldown, arrowPrefab, rb2D);
        BowDeadState dead = new BowDeadState(animator, coll);

        Func<bool> AgroCondition() => () => canSeePlayer || alerted || PlayerTooClose();
        Func<bool> DeathCondition() => () => IsDead;
        Func<bool> StartAimingCondition() => () => canSeePlayer;
        Func<bool> StopAimingCondition() => () => !canSeePlayer;

        stateMachine.AddTransition(idle, chase, AgroCondition());
        stateMachine.AddTransition(chase, aiming, StartAimingCondition());
        stateMachine.AddTransition(aiming, chase, StopAimingCondition());
        stateMachine.AddAnyTransition(dead, DeathCondition());

        stateMachine.SetState(idle);
    }

    protected override void Update()
    {
        canSeePlayer = CanSeePlayer();
        base.Update();
    }
}
