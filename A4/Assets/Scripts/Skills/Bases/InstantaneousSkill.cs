/// <summary>
/// An InstantaneousSkill is released instantaneously.
/// </summary>
public abstract class InstantaneousSkill : ConcreteSkill
{
    /// <summary>
    /// It is instantaneous, so one can release it iff it is not in cooldown
    /// </summary>
    /// <returns></returns>
    public override bool canReleaseSkill()
    {
        return !isInCooldown();
    }

    /// <summary>
    /// Release the skill.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">
    /// If it cannot be released now.
    /// </exception>
    /// 
    public void Release()
    {
        if (!canReleaseSkill())
        {
            throw new System.InvalidOperationException();
        }

        // perform the skill's effect
        onReleased();

        // start the cooldown timer
        cooldownTimer.start();
    }

    public override abstract void onReleased();
}