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

    public override abstract void onReleased(Vector3 position, GameObject target);
}