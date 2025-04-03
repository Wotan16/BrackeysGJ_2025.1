using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private GameObject damageColliderObject;
    [SerializeField] private List<Transform> raycastPoints;

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
            bool hitEnemy = false;
            LayerMask wallsMask = (1 << CustomLayerManager.wallsLayer);
            foreach (Transform t in raycastPoints)
            {
                Vector2 vectorToEnemy = collision.transform.position - t.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, vectorToEnemy.normalized, vectorToEnemy.magnitude, wallsMask);
                if (hit.collider == null)
                {
                    hitEnemy = true;
                    break;
                }
            }
            if (!hitEnemy)
                return;
            
            if (collision.TryGetComponent(out IDamageable health))
            {
                health.TakeDamage(1);
            }
        }
    }
}
