using UnityEngine;

/// <summary>
/// An entity that is damageable has a health and can be damaged.
/// </summary>
public interface IDamageable
{
    /*********************************** Methods ***********************************/
    public bool dead()
    {
        return getHealth() <= 0.0f;
    }

    public abstract float getHealth();

    /// <summary>
    /// Called when damage is dealt to this.
    /// Damage source is not needed in this game.
    /// </summary>
    /// <param name="dmg"></param>
    public abstract void onTakenDamage(float dmg);
}