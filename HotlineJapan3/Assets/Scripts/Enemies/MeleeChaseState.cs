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
    private float moveSpeed;
    private float attackRange;
    private float attackCooldown;

    private float attackCDDelta;
    private bool canAttack => attackCDDelta <= 0;

    public MeleeChaseState(MeleeEnemy enemy, AIDestinationSetter destinationSetter, FollowerEntity follower, Transform playerTransform,
        MeleeEnemyAnimator animator, float moveSpeed, float attackRange, float attackCooldown)
    {
        this.enemy = enemy;
        this.destinationSetter = destinationSetter;
        this.follower = follower;
        this.playerTransform = playerTransform;
        this.animator = animator;
        this.moveSpeed = moveSpeed;
        this.attackRange = attackRange;
        this.attackCooldown = attackCooldown;
    }

    public void FixedTick()
    {
        return;
    }

    public void OnEnter()
    {
        follower.enabled = true;
        follower.maxSpeed = moveSpeed;
        destinationSetter.target = playerTransform;
        animator.SetMoving(true);
    }

    public void OnExit()
    {
        destinationSetter.target = null;
        follower.enabled = false;
        animator.SetMoving(false);
    }

    public void Tick()
    {
        HandleAttackCooldown();

        if (!canAttack)
            return;

        if (InAttackRange())
        {
            attackCDDelta = attackCooldown;
            animator.Attack();
        }
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
