using UnityEngine;

public struct AttackHitInfo
{
    public enum AttackType
    {
        Melee,
        Projectile
    }

    public int damage;
    public AttackType type;
    public Vector2 knockback;

    public AttackHitInfo(int damage, AttackType type, Vector2 knockback)
    {
        this.damage = damage;
        this.type = type;
        this.knockback = knockback;
    }
}
