using NUnit.Framework;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class MeleePatrolState : IState
{
    private MeleeEnemy enemy;
    private AIDestinationSetter destinationSetter;
    private FollowerEntity follower;
    private MeleeEnemyAnimator animator;
    private List<Transform> patrolPoints;
    private float moveSpeed;

    private Transform activePoint;

    public MeleePatrolState(MeleeEnemy enemy, AIDestinationSetter destinationSetter, FollowerEntity follower, MeleeEnemyAnimator animator,
        List<Transform> patrolPoints, float moveSpeed)
    {
        this.enemy = enemy;
        this.destinationSetter = destinationSetter;
        this.follower = follower;
        this.animator = animator;
        this.patrolPoints = patrolPoints;
        this.moveSpeed = moveSpeed;
    }

    public void FixedTick()
    {
        return;
    }

    public void OnEnter()
    {
        if(activePoint == null)
        {
            activePoint = patrolPoints[0];
        }
        destinationSetter.target = activePoint;
        follower.maxSpeed = moveSpeed / 2;
        animator.SetMoving(true);
    }

    public void OnExit()
    {
        destinationSetter.target = null;
        animator.SetMoving(false);
    }

    public void Tick()
    {
        float distance = Vector2.Distance(activePoint.position, enemy.transform.position);
        if (distance < 0.6)
        {
            activePoint = patrolPoints[GetNextIndex()];
            destinationSetter.target = activePoint;
        }
    }

    private int GetIndexOfPoint(Transform point)
    {
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            if(patrolPoints[i] == point)
                return i;
        }
        return -1;
    }

    private int GetNextIndex()
    {
        int index = GetIndexOfPoint(activePoint);
        if(index + 1 == patrolPoints.Count)
        {
            return 0;
        }
        return index + 1;
    }
}
