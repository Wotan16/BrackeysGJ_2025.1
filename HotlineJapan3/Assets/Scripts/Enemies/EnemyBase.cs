using Pathfinding;
using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float visionAngle;
    [SerializeField] protected float visionRange;
    [SerializeField] protected float attackRange;

    [SerializeField] protected AIDestinationSetter destinationSetter;
    [SerializeField] protected HealthSystem healthSystem;
    protected Transform playerTransform;
    protected StateMachine stateMachine;

    private string currentState;

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        playerTransform = PlayerController.Instance.transform;

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDead += HealthSystem_OnDead;

        InitializeStateMachine();
    }

    protected virtual void Update()
    {
        stateMachine.Tick();
        currentState = stateMachine.CurrentState.ToString();
    }

    protected abstract void HealthSystem_OnDamaged();
        
    protected abstract void HealthSystem_OnDead();

    protected abstract void InitializeStateMachine();

    public virtual void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
    }

    protected virtual bool CanSeePlayer()
    {
        if (playerTransform == null)
            return false;

        bool inVisionRange = Vector3.Distance(transform.position, playerTransform.position) < visionRange;
        if (!inVisionRange)
            return false;

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        if(Vector2.Angle(transform.up, directionToPlayer) > visionAngle)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerTransform.position);
        if(hit.collider == null)
            return false;

        return hit.collider.CompareTag("Player");
    }

    private void OnDrawGizmos()
    {
        if (Debugger.ShowEnemyGizmos)
        {
            DrawGizmos();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Debugger.ShowEnemyGizmos)
        {
            DrawGizmos();
        }
    }

    protected virtual void DrawGizmos()
    {
        bool canSeePlayer = CanSeePlayer();
        Vector2 lookDirection = transform.up;
        float halfAngle = visionAngle / 2;
        Gizmos.color = canSeePlayer ? Color.magenta : Color.cyan;

        Quaternion leftRotation = Quaternion.AngleAxis(halfAngle, Vector3.back);
        Vector3 leftDirection = leftRotation * lookDirection;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection * visionRange);

        Quaternion rightRotation = Quaternion.AngleAxis(-halfAngle, Vector3.back);
        Vector3 rightDirection = rightRotation * lookDirection;
        Gizmos.DrawLine(transform.position, transform.position + rightDirection * visionRange);

        Gizmos.color = canSeePlayer ? Color.red : Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
}
