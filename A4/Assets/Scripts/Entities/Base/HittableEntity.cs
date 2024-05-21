/// <summary>
/// The player and the boss are hittable.
/// All obstacles are NOT hittable.
/// </summary>
public abstract class HittableEntity : Entity, IHittable
{
    /*********************************** Fields ***********************************/
    private float health;
    // at Start(), this is set to health to record the max value.
    private float maxHealth;
    private bool healthSet;

    /*********************************** Ctor ***********************************/
    public HittableEntity()
    {
        healthSet = false;
    }

    /*********************************** Methods ***********************************/
    // Because prefabs all call the default constructor,
    // I can only set the initial health using some other method.
    public void setInitialHealth(float initialHealth)
    {
        Utility.MyDebugAssert(!healthSet, "Can only set health once.");
        healthSet = true;

        Utility.MyDebugAssert(initialHealth > 0.0f, "the initial health must be positive.");
        health = initialHealth;
        maxHealth = health;
    }

    /*********************************** Observers ***********************************/

    public float getHealth()
    {
        return health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public float getHealthInPercent()
    {
        return 100.0f * health / maxHealth;
    }

    /*********************************** From IHittable ***********************************/

    /// <summary>
    /// Leave it virtual so that subclasses may override it.
    /// </summary>
    /// <param name="dmg"></param>
    public virtual void onHit(float dmg)
    {
        health -= dmg;
    }
}