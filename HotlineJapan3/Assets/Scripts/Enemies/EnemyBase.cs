using Pathfinding;
using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    public static event Action<EnemyBase> OnAnyEnemySpawned;
    public static event Action<EnemyBase> OnAnyEnemyDead;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float visionAngle;
    [SerializeField] protected float visionRange;
    [SerializeField] protected float attackRange;

    [SerializeField] protected AIDestinationSetter destinationSetter;
    [SerializeField] protected HealthSystem healthSystem;
    protected Transform playerTransform;
    protected StateMachine stateMachine;
    [SerializeField] protected Collider2D coll;
    [SerializeField] protected LayerMask obstacleMask;

    public string CurrentState;
    public bool IsDead => healthSystem.IsDead;
    public bool alerted = false;
    public float alertRange;
    [SerializeField] private float agroRange;

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

        OnAnyEnemySpawned?.Invoke(this);
    }

    protected virtual void Update()
    {
        stateMachine.Tick();
        CurrentState = stateMachine.CurrentState.ToString();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.FixedTick();
    }

    protected virtual void HealthSystem_OnDamaged()
    {
        AlertEnemiesAround(alertRange);
    }
        
    protected virtual void HealthSystem_OnDead()
    {
        OnAnyEnemyDead?.Invoke(this);
        AlertEnemiesAround(alertRange);
    }

    public void AlertEnemiesAround(float radius)
    {
        LayerMask charactersMask = (1 << CustomLayerManager.charactersLayer);
        
        Collider2D[] charactersAround = Physics2D.OverlapCircleAll(transform.position, radius, charactersMask);
        foreach (Collider2D characterCollider in charactersAround)
        {
            if (!characterCollider.TryGetComponent(out EnemyBase enemy))
                continue;

            enemy.alerted = true;
        }
    }

    protected abstract void InitializeStateMachine();

    public virtual void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
    }

    public virtual bool CanSeePlayer()
    {
        if (playerTransform == null)
            return false;

        bool inVisionRange = Vector3.Distance(transform.position, playerTransform.position) < visionRange;
        if (!inVisionRange)
            return false;

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float angleToPlayer = Vector2.Angle(transform.up, directionToPlayer);
        if (angleToPlayer > visionAngle / 2)
            return false;

        //float raycastOffset = 0.4f; //So raycast won't hit enemy collider;
        //Vector2 offsetPosition = transform.position + directionToPlayer * raycastOffset;
        //RaycastHit2D hit = Physics2D.Raycast(offsetPosition, directionToPlayer, visionRange, obstacleMask);
        //if(hit.collider == null)
        //    return false;

        //bool hasLineOfSight = hit.collider.CompareTag("Player");
        //return hasLineOfSight;
        return RaycastHitPlayer();
    }

    private bool RaycastHitPlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionToPlayer, visionRange, obstacleMask);
        RaycastHit2D playerHit = new RaycastHit2D();
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerHit = hit;
                break;
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerHit.point);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit == playerHit)
                continue;

            if (hit.collider == coll)
                continue;

            if (Vector2.Distance(transform.position, hit.point) < distanceToPlayer)
                return false;
        }
        return true;
    }

    public bool PlayerTooClose()
    {
        return Vector2.Distance(transform.position, playerTransform.position) <= agroRange;
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
        Gizmos.color = canSeePlayer ? Color.red : Color.blue;

        Quaternion leftRotation = Quaternion.AngleAxis(halfAngle, Vector3.back);
        Vector3 leftDirection = leftRotation * lookDirection;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection * visionRange);

        Quaternion rightRotation = Quaternion.AngleAxis(-halfAngle, Vector3.back);
        Vector3 rightDirection = rightRotation * lookDirection;
        Gizmos.DrawLine(transform.position, transform.position + rightDirection * visionRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, alertRange);
    }
}
