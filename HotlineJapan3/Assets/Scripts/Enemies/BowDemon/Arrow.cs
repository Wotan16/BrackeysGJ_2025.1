using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private bool isEnemy;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private float speed;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void SetArrow(Vector2 direction, bool isEnemy)
    {
        transform.up = direction;
        rb2D.linearVelocity = direction.normalized * speed;
        this.isEnemy = isEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemy)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController player = PlayerController.Instance;
                player.TakeDamage((Vector2 parryDirection) =>
                {
                    Arrow arrow = Instantiate(arrowPrefab, player.transform.position, player.transform.rotation);
                    arrow.SetArrow(parryDirection, false);
                });

                Destroy(gameObject);
                return;
            }

            if (collision.CompareTag("Enemy"))
            {
                return;
            }

            Destroy(gameObject);
        }
        else
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent(out IDamageable health))
                {
                    health.TakeDamage(1);
                }
            }

            if (collision.CompareTag("Player"))
            {
                return;
            }

            Destroy (gameObject);
        }
    }
}
