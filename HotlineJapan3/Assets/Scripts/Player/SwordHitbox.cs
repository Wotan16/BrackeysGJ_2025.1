using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private GameObject damageColliderObject;

    public void EnableHitbox()
    {
        damageColliderObject.SetActive(true);
    }

    public void DisableHitbox()
    {
        damageColliderObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out IDamageable health))
            {
                health.TakeDamage(1);
            }
        }
    }
}
