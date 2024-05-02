/// <summary>
/// The player and the boss are hittable.
/// All obstacles are NOT hittable.
/// </summary>
public abstract class HittableEntity : Entity, IHittable
{
    /*********************************** Fields ***********************************/
    private float health;

    /*********************************** Ctor ***********************************/
    public HittableEntity(float initialHealth)
    {
        Utility.MyDebugAssert(initialHealth > 0.0f, "initial health must be positive.");
        health = initialHealth;
    }

    /*********************************** From IHittable ***********************************/

    public float getHealth()
    {
        return health;
    }

    /// <summary>
    /// Leave it virtual so that subclasses may override it.
    /// </summary>
    /// <param name="dmg"></param>
    public virtual void onHit(int dmg)
    {
        health -= dmg;
    }
}