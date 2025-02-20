using Pathfinding;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    private FollowerEntity follower;    

    protected override void Start()
    {
        follower = GetComponent<FollowerEntity>();
        base.Start();
    }

    protected override void InitializeStateMachine()
    {
        throw new System.NotImplementedException();
    }

    protected override void HealthSystem_OnDamaged()
    {
        throw new System.NotImplementedException();
    }

    protected override void HealthSystem_OnDead()
    {
        throw new System.NotImplementedException();
    }
}
