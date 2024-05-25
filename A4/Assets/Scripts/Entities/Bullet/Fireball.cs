using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A bullet with the fire element, which can break ice shields.
/// </summary>
public class Fireball : Bullet
{
    protected override void onHitDamageable(DamageableEntity de)
    {
        // Destroy the ice shield
        GameObject shield = Utility.FindChildWithTag(de.gameObject, "IceShield");
        if(shield != null && !shield.IsDestroyed())
        {
            GameObject.Destroy(shield);
        }

        // revert the damage multiplier
        de.damageMultiplier = 1.0f;

        // base's
        base.onHitDamageable(de);
    }
}
