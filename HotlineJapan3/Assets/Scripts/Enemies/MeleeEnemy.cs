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
        return;
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
