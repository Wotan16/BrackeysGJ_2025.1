using UnityEngine;

public struct AttackInfo
{
    public enum AttackType
    {
        Melee,
        Projectile
    }

    public int damage;
    public AttackType type;

    public AttackInfo(int damage, AttackType type)
    {
        this.damage = damage;
        this.type = type;
    }
}
