using Pathfinding;
using System;
using UnityEngine;

public class MeleeChaseState : IState
{
    private MeleeEnemy enemy;
    private AIDestinationSetter destinationSetter;
    private FollowerEntity follower;
    private Transform playerTransform;
    private MeleeEnemyAnimator animator;
    private EnemySwordHitbox swordHitbox;
    private float moveSpeed;
    private float attackRange;
    private float attackCooldown;

    private float attackCDDelta;
    private bool canAttack => attackCDDelta <= 0;

    private bool preparingToAttack;
    public float hitboxActivationDelay = 0.15f;
    private float delayDelta;
    private float hitboxDuration = 0.1f;
    private float hitboxDurationDelta;

    
    public MeleeChaseState(MeleeEnemy enemy, AIDestinationSetter destinationSetter, FollowerEntity follower, Transform playerTransform,
        MeleeEnemyAnimator animator, float moveSpeed, float attackRange, float attackCooldown, EnemySwordHitbox swordHitbox)
    {
        this.enemy = enemy;
        this.destinationSetter = destinationSetter;
        this.follower = follower;
        this.playerTransform = playerTransform;
        this.animator = animator;
        this.moveSpeed = moveSpeed;
        this.attackRange = attackRange;
        this.attackCooldown = attackCooldown;
        this.swordHitbox = swordHitbox;
    }

    public void FixedTick()
    {
        return;
    }

    public void OnEnter()
    {
        enemy.AlertEnemiesAround(enemy.alertRange);
        follower.enabled = true;
        follower.maxSpeed = moveSpeed;
        destinationSetter.target = playerTransform;
        animator.SetMoving(true);
    }

    public void OnExit()
    {
        destinationSetter.target = null;
        follower.enabled = false;
        preparingToAttack = false;
        swordHitbox.DisableHitbox();
        animator.SetMoving(false);
    }

    public void Tick()
    {
        HandleAttackCooldown();
        HandleHitboxActivation();
        HandleHitboxActive();


        if (!canAttack)
            return;

        if (InAttackRange())
        {
            Attack();
        }
    }

    private void HandleHitboxActivation()
    {
        if (!preparingToAttack)
            return;

        if (delayDelta <= 0)
        {
            swordHitbox.EnableHitbox();
            preparingToAttack = false;
            return;
        }
        delayDelta -= Time.deltaTime;
    }

    private void HandleHitboxActive()
    {
        if (preparingToAttack)
            return;

        if(hitboxDurationDelta <= 0)
        {
            swordHitbox.DisableHitbox();
            return;
        }
        hitboxDurationDelta -= Time.deltaTime;
    }

    private void Attack()
    {
        AudioManager.PlaySound(SoundType.SwordSwing);   
        attackCDDelta = attackCooldown;
        hitboxDurationDelta = hitboxDuration;
        delayDelta = hitboxActivationDelay;
        preparingToAttack = true;
        animator.Attack();
    }

    private void HandleAttackCooldown()
    {
        if(attackCDDelta > 0)
            attackCDDelta -= Time.deltaTime;
    }

    private bool InAttackRange()
    {
        if (Vector2.Distance(enemy.transform.position, playerTransform.position) > attackRange)
            return false;

        if (!enemy.CanSeePlayer())
            return false;

        return true;
    }
}
