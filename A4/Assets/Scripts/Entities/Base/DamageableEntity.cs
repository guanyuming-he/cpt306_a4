/// <summary>
/// The player and the boss are damageable.
/// All obstacles are NOT damageable.
/// </summary>
public abstract class DamageableEntity : Entity, IDamageable
{
    /*********************************** Fields ***********************************/
    private float health;
    // at Start(), this is set to health to record the max value.
    private float maxHealth;
    private bool healthSet;

    /*********************************** Ctor ***********************************/
    public DamageableEntity()
    {
        // give some initial values
        health = 1.0f;
        maxHealth = 1.0f;
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

    /*********************************** Mutators ***********************************/
    /// <summary>
    /// When the entity is healed. Can be overridden to display effects.
    /// </summary>
    /// <param name="healedAmount"></param>
    public virtual void onHealed(float healedAmount)
    {
        Utility.MyDebugAssert(healedAmount > 0.0f, "can only heal a positive amount.");
        health += healedAmount;
    }

    /*********************************** From IDamageable ***********************************/

    /// <summary>
    /// Leave it virtual so that subclasses may override it to display effects.
    /// </summary>
    /// <param name="dmg"></param>
    public virtual void onTakenDamage(float dmg)
    {
        Utility.MyDebugAssert(dmg >= 0.0f, "can only have a nonnegative damage.");
        health -= dmg;

        if ((this as IDamageable).dead())
        {
            onDeath();
        }
    }

    /*********************************** Abstract methods ***********************************/
    /// <summary>
    /// Called in onTakenDamage() when the entity has died from the damage.
    /// </summary>
    public abstract void onDeath();
}