using UnityEngine;

/// <summary>
/// An entity that is hittable has a health and can be hit.
/// </summary>
public interface IHittable
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
    public abstract void onHit(float dmg);
}