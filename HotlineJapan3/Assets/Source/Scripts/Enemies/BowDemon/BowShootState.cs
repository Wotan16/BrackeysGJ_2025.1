using UnityEngine;

public class BowShootState : IState
{
    private BowEnemy enemy;
    private BowEnemyAnimator animator;
    private Arrow arrowPrefab;
    private Transform firePoint;
    public bool isShooting = false;

    public BowShootState(BowEnemy enemy, BowEnemyAnimator animator, Arrow arrowPrefab, Transform firePoint)
    {
        this.enemy = enemy;
        this.animator = animator;
        this.arrowPrefab = arrowPrefab;
        this.firePoint = firePoint;
    }

    public void OnEnter()
    {
        isShooting = true;
        animator.Shoot();

        animator.OnBowRelease += Animator_OnBowRelease;
        animator.OnShootAnimationEnded += Animator_OnShootAnimationEnded;
    }

    private void Animator_OnShootAnimationEnded()
    {
        isShooting = false;
    }

    private void Animator_OnBowRelease()
    {
        ShootArrow();
    }

    public void OnExit()
    {
        animator.OnBowRelease -= Animator_OnBowRelease;
        animator.OnShootAnimationEnded -= Animator_OnShootAnimationEnded;
        return;
    }

    public void Tick()
    {
        return;
    }

    public void FixedTick()
    {
        enemy.RotateTowardsPlayer();
    }

    private void ShootArrow()
    {
        Vector2 directionToPlayer = PlayerController.Instance.transform.position - firePoint.position;
        Arrow arrow = GameObject.Instantiate(arrowPrefab, firePoint.position, enemy.transform.rotation);
        arrow.SetArrow(directionToPlayer, true, enemy.transform);
        AudioManager.PlaySound(SoundType.BowShot);
    }
}
