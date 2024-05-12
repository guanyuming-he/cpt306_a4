/// <summary>
/// The player and the boss are hittable.
/// All obstacles are NOT hittable.
/// </summary>
public abstract class HittableEntity : Entity, IHittable
{
    /*********************************** Fields ***********************************/
    private float health;
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