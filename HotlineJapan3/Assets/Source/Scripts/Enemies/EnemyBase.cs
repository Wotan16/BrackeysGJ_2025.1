using Pathfinding;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

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
    [SerializeField] protected Rigidbody2D rb2D;

    public string CurrentState;
    public bool IsDead => healthSystem.IsDead;
    public bool alerted = false;
    public float alertRange;
    [SerializeField] private float agroRange;
    [SerializeField] private GameObject shadowsObject;

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
        shadowsObject.SetActive(false);
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
        AudioManager.PlaySound(SoundType.SwordHitImpact);
    }

    public void RotateTowardsPlayer()
    {
        Vector2 playerPosition = PlayerController.Instance.transform.position;
        Vector2 direction = (playerPosition - rb2D.position).normalized;
        float rotation = Vector2.Angle(Vector2.up, direction);
        rotation = playerPosition.x < rb2D.position.x ? rotation : -rotation;
        rb2D.MoveRotation(rotation);
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

        if(playerHit.collider == null)
            return false;

        float distanceToPlayer = Vector2.Distance(transform.position, playerHit.point);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == playerHit.collider)
                continue;

            if (hit.collider == coll)
                continue;

            if(hit.collider.CompareTag("Enemy"))
                continue;

            if (hit.distance < distanceToPlayer)
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
        DrawGizmos();
    }

    private void OnDrawGizmosSelected()
    {
        DrawAlertRange();
        DrawVisionCone();
    }

    protected virtual void DrawGizmos()
    {
        if (Debugger.ShowEnemyVision)
        {
            DrawVisionCone();
        }

        if (Debugger.ShowEnemyAgroRange)
        {
            DrawAlertRange();
        }
    }

    private void DrawAlertRange()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, alertRange);
    }

    private void DrawVisionCone()
    {
        bool canSeePlayer = CanSeePlayer();
        Vector2 lookDirection = transform.up;
        float halfAngle = visionAngle / 2;
        Gizmos.color = canSeePlayer ? Color.red : Color.blue;
        int numberOfEdges = 48;

        Quaternion leftRotation = Quaternion.AngleAxis(halfAngle, Vector3.back);
        Vector3 leftDirection = leftRotation * lookDirection;
        RaycastHit2D[] leftHits = Physics2D.RaycastAll(transform.position, leftDirection, visionRange, obstacleMask);
        Vector2 lastVertexPos = GetClosestHitPosition(leftHits, leftDirection);
        Gizmos.DrawLine(transform.position, lastVertexPos);

        for (int i = 1; i <= numberOfEdges; i++)
        {
            float raycastAngle = -visionAngle * (i / (float)numberOfEdges);
            Quaternion addedRotation = Quaternion.AngleAxis(raycastAngle, Vector3.back);
            Vector3 direction = addedRotation * leftDirection;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, visionRange, obstacleMask);
            Vector2 nextVertexPos = GetClosestHitPosition(hits, direction);
            Gizmos.DrawLine(lastVertexPos, nextVertexPos);
            lastVertexPos = nextVertexPos;
        }

        Gizmos.DrawLine(transform.position, lastVertexPos);
    }

    private Vector3 GetClosestHitPosition(RaycastHit2D[] hits, Vector3 direction)
    {
        float maxDistance = visionRange;
        RaycastHit2D closestHit = new RaycastHit2D();
        foreach (var hit in hits)
        {
            if(hit.collider == null)
                continue;
            if(hit.collider == coll)
                continue;
            if(hit.distance < maxDistance)
            {
                maxDistance = hit.distance;
                closestHit = hit;
            }
        }

        if(closestHit.collider == null)
        {
            return transform.position + direction * visionRange;
        }

        return closestHit.point;
    }
}
