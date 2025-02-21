using System;
using UnityEngine;

public class EnemySwordHitbox : MonoBehaviour
{
    public event Action<PlayerController> OnHitPlayer;
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
        if (collision.CompareTag("Player"))
        {
            OnHitPlayer?.Invoke(PlayerController.Instance);
        }
    }
}
