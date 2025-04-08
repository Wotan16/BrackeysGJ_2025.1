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
    public Transform attackerTransform;

    public AttackHitInfo(int damage, AttackType type, Transform attackerTransform, Vector2 knockback)
    {
        this.damage = damage;
        this.type = type;
        this.knockback = knockback;
        this.attackerTransform = attackerTransform;
    }
}
