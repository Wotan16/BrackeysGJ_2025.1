using Pathfinding;
using UnityEngine;

public class BowChaseState : IState
{
    private BowEnemy enemy;
    private AIDestinationSetter destinationSetter;
    private FollowerEntity follower;
    private Transform playerTransform;
    private BowEnemyAnimator animator;
    private float moveSpeed;

    public BowChaseState(BowEnemy enemy, AIDestinationSetter destinationSetter, FollowerEntity follower, Transform playerTransform, BowEnemyAnimator animator, float moveSpeed)
    {
        this.enemy = enemy;
        this.destinationSetter = destinationSetter;
        this.follower = follower;
        this.playerTransform = playerTransform;
        this.animator = animator;
        this.moveSpeed = moveSpeed;
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
        animator.SetMoving(false);
    }

    public void Tick()
    {
        return;
    }
}
