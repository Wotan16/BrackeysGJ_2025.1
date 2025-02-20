using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDead;
    public event Action OnHealthChanged;
    public event Action OnDamaged;
    public event Action OnHealed;

    [SerializeField]
    protected int health = 100;
    public int Health { get { return health; } }

    protected int healthMax;
    public int HealthMax { get { return healthMax; } }

    public bool IsDead { get { return health <= 0; } }
    public bool IsHealthFull { get { return health >= healthMax; } }

    protected virtual void Awake()
    {
        healthMax = health;
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            Die();
            return;
        }

        OnDamaged?.Invoke();
        OnHealthChanged?.Invoke();
    }

    public virtual void TakeHeal(int amount)
    {
        health += amount;

        if (health > healthMax)
            health = healthMax;

        OnHealed?.Invoke();
        OnHealthChanged?.Invoke();
    }

    protected virtual void Die()
    {
        OnDead?.Invoke();
        OnHealthChanged?.Invoke();
    }

    public virtual float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
