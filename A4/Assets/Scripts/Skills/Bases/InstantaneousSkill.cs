using UnityEngine;

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
    /// <param name="position">
    /// the position where the skill is to be released.
    /// may or may not be used.
    /// </param>
    /// <param name="target">the target of the skill, can be null if not used.</param>
    /// <exception cref="System.InvalidOperationException">
    /// If it cannot be released now.
    /// </exception>
    /// 
    public void Release(Vector3 position, GameObject target)
    {
        if (!canReleaseSkill())
        {
            throw new System.InvalidOperationException();
        }

        // perform the skill's effect
        onReleased(position, target);

        // start the cooldown timer
        cooldownTimer.start();
    }

    public override abstract void onReleased(Vector3 position, GameObject target);
}