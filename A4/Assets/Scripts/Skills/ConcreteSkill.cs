using UnityEngine;

/// <summary>
/// A ConcreteSkill is a concrete (opposed to the most abstract form, an interface) 
/// implementation of ISkill that defines a few necessary fields and the basic logic.
/// </summary>
public abstract class ConcreteSkill : MonoBehaviour, ISkill
{
    /*********************************** Fields ***********************************/
    private bool selected;
    private Timer cooldownTimer;

    /*********************************** Ctor ***********************************/
    public ConcreteSkill()
    {
        selected = false;
        // inited later as the skill information is not available at construction for a Mono script.
        cooldownTimer = null;
    }

    /*********************************** Interface ISkill ***********************************/

    /// <returns>true if it is not in cooldown or it is not current releasing a skill.</returns>
    public abstract bool canReleaseSkill();

    public bool isSelected()
    {
        return selected;
    }

    public abstract void onDeselected();
    public abstract void onSelected();

    public abstract void startReleasing();
    public abstract void stopReleasing();

    /*********************************** Mono ***********************************/
}