using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    private FollowerEntity follower;

    [Header("Sword enemy params")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float stunDuration;
    public bool KnockedDown = false;

    [Header("Sword enemy references")]
    [SerializeField] private MeleeEnemyAnimator animator;
    [SerializeField] private EnemySwordHitbox swordHitbox;
    [SerializeField] private List<Transform> patrolPoints;

    protected override void Start()
    {
        follower = GetComponent<FollowerEntity>();
        base.Start();

        swordHitbox.OnHitPlayer += SwordHitbox_OnHitPlayer;
    }

    private void SwordHitbox_OnHitPlayer(PlayerController player)
    {
        player.TakeDamage(new AttackHitInfo(1, AttackHitInfo.AttackType.Melee, transform, Vector2.zero),
            (Vector2 parryDireciton) =>
        {
            AudioManager.PlaySound(SoundType.SwordClash);
            KnockedDown = true;
        });
    }

    protected override void InitializeStateMachine()
    {
        EmptyState idle = new EmptyState();
        MeleePatrolState patrol = new MeleePatrolState(this, destinationSetter, follower, animator, patrolPoints, moveSpeed);
        MeleeChaseState chase = new MeleeChaseState(this, destinationSetter, follower, playerTransform, animator, moveSpeed, attackRange, attackCooldown, swordHitbox);
        KnockedDownState knockedDownState = new KnockedDownState(this, animator, rb2D, stunDuration);
        MeleeDeadState dead = new MeleeDeadState(animator, coll);

        Func<bool> AgroCondition() => () => CanSeePlayer() || alerted || PlayerTooClose();
        Func<bool> DeathCondition() => () => IsDead;
        Func<bool> KnockedDownCondition() => () => KnockedDown;
        Func<bool> GetUpCondition() => () => !KnockedDown;

        stateMachine.AddTransition(idle, chase, AgroCondition());
        stateMachine.AddTransition(patrol, chase, AgroCondition());
        stateMachine.AddTransition(knockedDownState, chase, GetUpCondition());
        stateMachine.AddAnyTransition(dead, DeathCondition());
        stateMachine.AddAnyTransition(knockedDownState, KnockedDownCondition());

        if (patrolPoints.Count > 1)
            stateMachine.SetState(patrol);
        else
            stateMachine.SetState(idle);
    }

    protected override void DrawGizmos()
    {
        base.DrawGizmos();

        if (patrolPoints.Count > 1)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < patrolPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
            }

            Gizmos.DrawLine(patrolPoints.Last().position, patrolPoints.First().position);
        }
    }
}
