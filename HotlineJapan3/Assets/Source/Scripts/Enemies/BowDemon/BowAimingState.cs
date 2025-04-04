using UnityEngine;

public class BowAimingState : IState
{
    private BowEnemy enemy;
    private BowEnemyAnimator animator;
    private Arrow arrowPrefab;
    private Rigidbody2D rb2D;
    private float timeToAim;
    private float attackCooldown;

    private float timeToAimDelta;
    private float attackCDDelta;

    private bool isAiming;

    public BowAimingState(BowEnemy enemy, BowEnemyAnimator animator, float timeToAim, float attackCooldown, Arrow arrowPrefab, Rigidbody2D rb2D)
    {
        this.animator = animator;
        this.timeToAim = timeToAim;
        this.attackCooldown = attackCooldown;
        this.arrowPrefab = arrowPrefab;
        this.enemy = enemy;
        this.rb2D = rb2D;
    }

    public void OnEnter()
    {
        animator.SetAiming(true);
        //isAiming = true;
    }

    public void OnExit()
    {
        animator.SetAiming(false);
    }

    public void Tick()
    {
        if (isAiming)
        {
            if(timeToAimDelta <= 0)
            {
                attackCDDelta = attackCooldown;
                ShootArrow();
                isAiming = false;
            }
            timeToAimDelta -= Time.deltaTime;
            return;
        }

        if (attackCDDelta <= 0)
        {
            timeToAimDelta = timeToAim;
            isAiming = true;
        }
        attackCDDelta -= Time.deltaTime;
    }

    private void ShootArrow()
    {
        Vector2 directionToPlayer = PlayerController.Instance.transform.position - enemy.transform.position;
        Arrow arrow = GameObject.Instantiate(arrowPrefab, enemy.transform.position, enemy.transform.rotation);
        arrow.SetArrow(directionToPlayer, true, enemy.transform);
        AudioManager.PlaySound(SoundType.BowShot);
    }

    private void RotateTowardsPlayer()
    {
        Vector2 playerPosition = PlayerController.Instance.transform.position;
        Vector2 direction = (playerPosition - rb2D.position).normalized;
        float rotation = Vector2.Angle(Vector2.up, direction);
        rotation = playerPosition.x < rb2D.position.x ? rotation : -rotation;
        rb2D.MoveRotation(rotation);
    }
        
    public void FixedTick()
    {
        RotateTowardsPlayer();
    }
}
