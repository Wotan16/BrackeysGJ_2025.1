using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform shooterTransform;
    private Rigidbody2D rb2D;
    public bool isEnemy;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private float speed;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void SetArrow(Vector2 direction, bool isEnemy, Transform shooterTransform)
    {
        transform.up = direction;
        rb2D.linearVelocity = direction.normalized * speed;
        this.isEnemy = isEnemy;
        this.shooterTransform = shooterTransform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
            return;

        if (collision.gameObject.layer == CustomLayerManager.wallsLayer)
        {
            Destroy(gameObject);
            return;
        }

        if (isEnemy)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController player = PlayerController.Instance;
                if(player.TakeDamage(new AttackHitInfo(1, AttackHitInfo.AttackType.Projectile, Vector2.zero), OnParried))
                {
                    Destroy(gameObject);
                }
                return;
            }
        }
        else
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent(out IDamageable health))
                {
                    health.TakeDamage(new AttackHitInfo(1, AttackHitInfo.AttackType.Projectile, transform.up * 100f));
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }

    private void OnParried(Vector2 parryDirection)
    {
        AudioManager.PlaySound(SoundType.ArrowParry);
        Destroy(gameObject);
    }

    public void Deflect(GameObject deflectCharacterObject)
    {
        Vector2 deflectDirection = shooterTransform.position - transform.position;
        Arrow arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        arrow.SetArrow(deflectDirection, deflectCharacterObject.CompareTag("Enemy"), deflectCharacterObject.transform);
        AudioManager.PlaySound(SoundType.ArrowParry);
        Destroy(gameObject);
    }
}
